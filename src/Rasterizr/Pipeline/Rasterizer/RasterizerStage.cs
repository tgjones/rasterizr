using System;
using System.Collections.Generic;
using Rasterizr.Math;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.PixelShader;
using Rasterizr.Pipeline.VertexShader;

namespace Rasterizr.Pipeline.Rasterizer
{
	public class RasterizerStage
	{
		private readonly VertexShaderStage _vertexShader;
		private readonly PixelShaderStage _pixelShader;
		private readonly OutputMergerStage _outputMerger;
		private Viewport[] _viewports;

		public RasterizerState State { get; set; }

		public RasterizerStage(Device device,  VertexShaderStage vertexShader, PixelShaderStage pixelShader, OutputMergerStage outputMerger)
		{
			_vertexShader = vertexShader;
			_pixelShader = pixelShader;
			_outputMerger = outputMerger;

			State = new RasterizerState(device, RasterizerStateDescription.Default);
		}

		public void SetViewports(params Viewport[] viewports)
		{
			_viewports = viewports;
		}

		internal IEnumerable<FragmentQuad> Execute(IEnumerable<InputAssemblerPrimitiveOutput> inputs, PrimitiveTopology primitiveTopology)
		{
			foreach (var primitive in inputs)
			{
				for (int i = 0; i < primitive.Vertices.Length; i++)
				{
					PerspectiveDivide(ref primitive.Vertices[i].Position);
					ToScreenCoordinates(ref primitive.Vertices[i].Position);
				}

				switch (primitiveTopology)
				{
					case PrimitiveTopology.PointList:
						break;
					case PrimitiveTopology.LineList:
					case PrimitiveTopology.LineStrip :
						break;
					case PrimitiveTopology.TriangleList:
					case PrimitiveTopology.TriangleStrip :
						foreach (var fragmentQuad in RasterizeTriangle(primitive))
							yield return fragmentQuad;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		private IEnumerable<FragmentQuad> RasterizeTriangle(InputAssemblerPrimitiveOutput primitive)
		{
			var screenBounds = GetTriangleScreenBounds(ref primitive);

			// Scan pixels in target area, checking if they are inside the triangle.
			// If they are, calculate the coverage.
			var p0 = primitive.Vertices[0].Position;
			var p1 = primitive.Vertices[1].Position;
			var p2 = primitive.Vertices[2].Position;

			// Calculate start and end positions. Because of the need to calculate derivatives
			// in the pixel shader, we require that fragment quads always have even numbered
			// coordinates (both x and y) for the top-left fragment.
			int startY = NearestEvenNumber(screenBounds.MinY);
			int startX = NearestEvenNumber(screenBounds.MinX);

			for (int y = startY; y <= screenBounds.MaxY; y += 2)
				for (int x = startX; x <= screenBounds.MaxX; x += 2)
				{
					// First check whether any fragments in this quad are covered. If not, we don't
					// need to any (expensive) interpolation of attributes.
					var fragmentQuad = new FragmentQuad
					{
						Fragment0 = new Fragment(x, y, FragmentQuadLocation.TopLeft),
						Fragment1 = new Fragment(x + 1, y, FragmentQuadLocation.TopRight),
						Fragment2 = new Fragment(x, y + 1, FragmentQuadLocation.BottomLeft),
						Fragment3 = new Fragment(x + 1, y + 1, FragmentQuadLocation.BottomRight)
					};

					// Check all samples to determine whether they are inside the triangle.
					bool anyCoveredSamples = CalculateSampleCoverage(ref fragmentQuad.Fragment0, ref p0, ref p1, ref p2);
					anyCoveredSamples = anyCoveredSamples || CalculateSampleCoverage(ref fragmentQuad.Fragment1, ref p0, ref p1, ref p2);
					anyCoveredSamples = anyCoveredSamples || CalculateSampleCoverage(ref fragmentQuad.Fragment2, ref p0, ref p1, ref p2);
					anyCoveredSamples = anyCoveredSamples || CalculateSampleCoverage(ref fragmentQuad.Fragment3, ref p0, ref p1, ref p2);
					if (!anyCoveredSamples)
						continue;

					// Otherwise, we do have at least one fragment with covered samples, so continue
					// with interpolation. We need to interpolate values for all fragments in this quad,
					// even though they may not all be covered, because we need all four fragments in order
					// to calculate derivatives correctly.
					InterpolateFragmentData(ref fragmentQuad.Fragment0, ref primitive, ref p0, ref p1, ref p2);
					InterpolateFragmentData(ref fragmentQuad.Fragment1, ref primitive, ref p0, ref p1, ref p2);
					InterpolateFragmentData(ref fragmentQuad.Fragment2, ref primitive, ref p0, ref p1, ref p2);
					InterpolateFragmentData(ref fragmentQuad.Fragment3, ref primitive, ref p0, ref p1, ref p2);

					yield return fragmentQuad;
				}
		}

		private void InterpolateFragmentData(ref Fragment fragment, ref InputAssemblerPrimitiveOutput triangle,
			ref Vector4 p0, ref Vector4 p2, ref Vector4 p1)
		{
			var pixelCenter = new Vector2(fragment.X + 0.5f, fragment.Y + 0.5f);

			// Calculate alpha, beta, gamma for pixel center.
			float alpha = ComputeFunction(pixelCenter.X, pixelCenter.Y, ref p1, ref p2)
				/ ComputeFunction(p0.X, p0.Y, ref p1, ref p2);
			float beta = ComputeFunction(pixelCenter.X, pixelCenter.Y, ref p2, ref p0)
				/ ComputeFunction(p1.X, p1.Y, ref p2, ref p0);
			float gamma = ComputeFunction(pixelCenter.X, pixelCenter.Y, ref p0, ref p1)
				/ ComputeFunction(p2.X, p2.Y, ref p0, ref p1);

			// Create pixel shader input.
			fragment.Data = CreatePixelShaderInput(ref triangle, beta, alpha, gamma);
		}

		private static void PerspectiveDivide(ref Vector4 position)
		{
			position.X /= position.W;
			position.Y /= position.W;
			position.Z /= position.W;
		}

		/// <summary>
		/// Formulae from http://msdn.microsoft.com/en-us/library/bb205126(v=vs.85).aspx
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		private void ToScreenCoordinates(ref Vector4 position)
		{
			var viewport = _viewports[0];
			position.X = (position.X + 1) * viewport.Width * 0.5f + viewport.TopLeftX;
			position.Y = (1 - position.Y) * viewport.Height * 0.5f + viewport.TopLeftY;
			position.Z = viewport.MinDepth + position.Z * (viewport.MaxDepth - viewport.MinDepth);
		}

		private static Box2D GetTriangleScreenBounds(ref InputAssemblerPrimitiveOutput triangle)
		{
			float minX = float.MaxValue, minY = float.MaxValue;
			float maxX = float.MinValue, maxY = float.MinValue;

			foreach (var vertex in triangle.Vertices)
			{
				var position = vertex.Position;

				if (position.X < minX)
					minX = position.X;
				if (position.X > maxX)
					maxX = position.X;

				if (position.Y < minY)
					minY = position.Y;
				if (position.Y > maxY)
					maxY = position.Y;
			}

			return new Box2D((int)minX, (int)minY, (int)maxX, (int)maxY);
		}

		private static int NearestEvenNumber(int value)
		{
			return (value % 2 == 0) ? value : value - 1;
		}

		private static float ComputeFunction(float x, float y, ref Vector4 pa, ref Vector4 pb)
		{
			return (pa.Y - pb.Y) * x + (pb.X - pa.X) * y + pa.X * pb.Y - pb.X * pa.Y;
		}

		private Vector4[] CreatePixelShaderInput(ref InputAssemblerPrimitiveOutput triangle, float beta, float alpha, float gamma)
		{
			// TODO: Cache as much of this as possible.
			// Calculate interpolated attribute values for this fragment.
			var vertexShaderOutputSignature = _vertexShader.Shader.Bytecode.OutputSignature;
			var pixelShaderInputSignature = _pixelShader.Shader.Bytecode.InputSignature;
			var result = new Vector4[pixelShaderInputSignature.Parameters.Count];
			for (int i = 0; i < pixelShaderInputSignature.Parameters.Count; i++)
			{
				var parameter = pixelShaderInputSignature.Parameters[i];

				// Grab values from vertex shader outputs.
				var outputParameterRegister = vertexShaderOutputSignature.Parameters.FindRegister(
					parameter.SemanticName, parameter.SemanticIndex);
				var v0Value = triangle.Vertices[0].Data[outputParameterRegister];
				var v1Value = triangle.Vertices[1].Data[outputParameterRegister];
				var v2Value = triangle.Vertices[2].Data[outputParameterRegister];

				// Interpolate values.
				const bool isPerspectiveCorrect = true; // TODO
				Vector4 interpolatedValue = (isPerspectiveCorrect)
					? InterpolationUtility.Perspective(alpha, beta, gamma, ref v0Value, ref v1Value, ref v2Value,
						triangle.Vertices[0].Position.W, triangle.Vertices[1].Position.W, triangle.Vertices[2].Position.W)
					: InterpolationUtility.Linear(alpha, beta, gamma, ref v0Value, ref v1Value, ref v2Value);

				// Set value onto pixel shader input.
				result[i] = interpolatedValue;

				// TODO: Do something different if input parameter is a system value.
			}
			return result;
		}

		private Vector2 GetSamplePosition(int x, int y, int sampleIndex)
		{
			return MultiSamplingUtility.GetSamplePosition(_outputMerger.MultiSampleCount, x, y, sampleIndex);
		}

		private bool CalculateSampleCoverage(ref Fragment fragment, ref Vector4 p0, ref Vector4 p2, ref Vector4 p1)
		{
			int multiSampleCount = _outputMerger.MultiSampleCount;
			bool anyCoveredSamples = false;
			if (multiSampleCount > 0)
				anyCoveredSamples = CalculateSampleCoverage(ref fragment, 0, ref p0, ref p1, ref p2, out fragment.Samples.Sample0);
			if (multiSampleCount > 1)
				anyCoveredSamples = anyCoveredSamples || CalculateSampleCoverage(ref fragment, 1, ref p0, ref p1, ref p2, out fragment.Samples.Sample1);
			if (multiSampleCount > 2)
				anyCoveredSamples = anyCoveredSamples || CalculateSampleCoverage(ref fragment, 2, ref p0, ref p1, ref p2, out fragment.Samples.Sample2);
			if (multiSampleCount > 3)
				anyCoveredSamples = anyCoveredSamples || CalculateSampleCoverage(ref fragment, 3, ref p0, ref p1, ref p2, out fragment.Samples.Sample3);
			fragment.Samples.AnyCovered = anyCoveredSamples;
			return anyCoveredSamples;
		}

		private bool CalculateSampleCoverage(ref Fragment fragment, int sampleIndex,
			ref Vector4 p0, ref Vector4 p2, ref Vector4 p1, out Sample sample)
		{
			// Is this pixel inside triangle?
			Vector2 samplePosition = GetSamplePosition(fragment.X, fragment.Y, sampleIndex);

			float depth;
			bool covered = IsSampleInsideTriangle(ref p0, ref p1, ref p2, ref samplePosition, out depth);
			sample = new Sample
			{
				Covered = covered,
				Depth = depth
			};
			return covered;
		}

		private bool IsSampleInsideTriangle(ref Vector4 p0, ref Vector4 p1, ref Vector4 p2, ref Vector2 samplePosition, out float depth)
		{
			// TODO: Use fill convention.

			// Calculate alpha, beta, gamma for this sample position.
			float alpha = ComputeFunction(samplePosition.X, samplePosition.Y, ref p1, ref p2)
				/ ComputeFunction(p0.X, p0.Y, ref p1, ref p2);
			float beta = ComputeFunction(samplePosition.X, samplePosition.Y, ref p2, ref p0)
				/ ComputeFunction(p1.X, p1.Y, ref p2, ref p0);
			float gamma = ComputeFunction(samplePosition.X, samplePosition.Y, ref p0, ref p1)
				/ ComputeFunction(p2.X, p2.Y, ref p0, ref p1);

			// Calculate depth.
			// TODO: Does this only need to be calculated if the sample is inside the triangle?
			depth = InterpolationUtility.Linear(alpha, beta, gamma, p0.Z, p1.Z, p2.Z);

			// If any of these tests fails, the current pixel is not inside the triangle.
			// TODO: Only need to test if > 1?
			if (alpha < 0 || alpha > 1 || beta < 0 || beta > 1 || gamma < 0 || gamma > 1)
			{
				depth = 0;
				return false;
			}

			// The exact value to compare against depends on fill mode - if we're rendering wireframe,
			// then check whether sample position is within the "wireframe threshold" (i.e. 1 pixel) of an edge.
			switch (State.Description.FillMode)
			{
				case FillMode.Solid:
					return true;
				case FillMode.Wireframe:
					const float wireframeThreshold = 0.00001f;
					return alpha < wireframeThreshold || beta < wireframeThreshold || gamma < wireframeThreshold;
				default:
					throw new NotSupportedException();
			}
		}
	}

	internal struct Box2D
	{
		public int MinX;
		public int MinY;
		public int MaxX;
		public int MaxY;

		public Box2D(int minX, int minY, int maxX, int maxY)
		{
			MinX = minX;
			MinY = minY;
			MaxX = maxX;
			MaxY = maxY;
		}
	}
}