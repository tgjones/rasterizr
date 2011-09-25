using System;
using System.Collections.Generic;
using Nexus;
using Nexus.Graphics;
using Rasterizr.Core.OutputMerger;
using Rasterizr.Core.Rasterizer.Interpolation;
using Rasterizr.Core.ShaderCore;
using Rasterizr.Core.ShaderCore.GeometryShader;
using Rasterizr.Core.ShaderCore.PixelShader;
using Rasterizr.Core.ShaderCore.VertexShader;

namespace Rasterizr.Core.Rasterizer
{
	public class RasterizerStage : PipelineStageBase<IVertexShaderOutput, Fragment>
	{
		private readonly VertexShaderStage _vertexShaderStage;
		private readonly PixelShaderStage _pixelShaderStage;
		private readonly OutputMergerStage _outputMerger;

		private readonly PerspectiveDividerSubStage _perspectiveDivider;
		private readonly ClipperSubStage _clipper;
		private readonly CullerSubStage _culler;
		private readonly ScreenMapperSubStage _screenMapper;

		private readonly List<IVertexShaderOutput> _clipperOutputs;
		private readonly List<IVertexShaderOutput> _cullerOutputs;

		private CullMode _cullMode;
		private Viewport3D _viewport;

		public CullMode CullMode
		{
			get { return _cullMode; }
			set
			{
				_cullMode = value;
				_culler.CullMode = value;
			}
		}

		public FillMode FillMode { get; set; }

		// TODO: Hook ths property up. Should only affect line rasterization
		// (from http://msdn.microsoft.com/en-us/library/bb694530(v=vs.85).aspx)
		public bool MultiSampleAntiAlias { get; set; }

		public Viewport3D Viewport
		{
			get { return _viewport; }
			set
			{
				_viewport = value;
				_screenMapper.Viewport = value;
			}
		}

		public RasterizerStage(VertexShaderStage vertexShaderStage, PixelShaderStage pixelShaderStage, OutputMergerStage outputMerger)
		{
			_vertexShaderStage = vertexShaderStage;
			_pixelShaderStage = pixelShaderStage;
			_outputMerger = outputMerger;

			_perspectiveDivider = new PerspectiveDividerSubStage();
			_clipper = new ClipperSubStage();
			_culler = new CullerSubStage();
			_screenMapper = new ScreenMapperSubStage();

			_clipperOutputs = new List<IVertexShaderOutput>();
			_cullerOutputs = new List<IVertexShaderOutput>();

			CullMode = CullMode.CullCounterClockwiseFace;
			FillMode = FillMode.Solid;
		}

		public override void Run(List<IVertexShaderOutput> inputs, List<Fragment> outputs)
		{
			_clipperOutputs.Clear();
			_cullerOutputs.Clear();

			// Do perspective divide.
			_perspectiveDivider.Process(inputs);

			// Clip to viewport.
			_clipper.Process(inputs, _clipperOutputs);

			// Cull backward facing triangles.
			_culler.Process(_clipperOutputs, _cullerOutputs);

			// Map to screen coordinates.
			_screenMapper.Process(_cullerOutputs);
			inputs = _cullerOutputs;

			// Rasterize.
			for (int i = 0; i < inputs.Count; i += 3)
			{
				IVertexShaderOutput v1 = inputs[i + 0];
				IVertexShaderOutput v2 = inputs[i + 1];
				IVertexShaderOutput v3 = inputs[i + 2];

				var triangle = new TrianglePrimitive(v1, v2, v3);

				// Determine screen bounds of triangle so we know which pixels to test.
				Box2D screenBounds = GetScreenBounds(triangle);

				// Scan pixels in target area, checking if they are inside the triangle.
				// If they are, calculate the coverage.
				RasterizeTriangle(screenBounds, outputs, triangle);
			}
		}

		private static Box2D GetScreenBounds(TrianglePrimitive triangle)
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

			return new Box2D(new IntPoint2D((int) minX, (int) minY),
				new IntPoint2D((int) maxX, (int) maxY));
		}

		public Point2D GetSamplePosition(int x, int y, int sampleIndex)
		{
			return MultiSamplingUtility.GetSamplePosition(
				_outputMerger.RenderTarget.MultiSampleCount,
				x, y, sampleIndex);
		}

		private void RasterizeTriangle(Box2D screenBounds, List<Fragment> outputs, TrianglePrimitive triangle)
		{
			Point4D p0 = triangle.V1.Position;
			Point4D p1 = triangle.V2.Position;
			Point4D p2 = triangle.V3.Position;

			// TODO: Parallelize this?
			// TODO: Do I really need to pad the screen bounds by 1 on each side?
			for (int x = screenBounds.Min.X - 1; x <= screenBounds.Max.X + 1; x++)
				for (int y = screenBounds.Min.Y - 1; y <= screenBounds.Max.Y + 1; y++)
				{
					// Check all samples to determine whether they are inside the triangle.
					var samples = new SampleCollection();
					var anyCoveredSamples = CalculateSampleCoverage(x, y, p0, p2, p1, samples);

					if (!anyCoveredSamples) 
						continue;

					Point2D pixelCenter = new Point2D(x + 0.5f, y + 0.5f);

					// Calculate alpha, beta, gamma for pixel center.
					float alpha = ComputeFunction(pixelCenter.X, pixelCenter.Y, p1, p2) / ComputeFunction(p0.X, p0.Y, p1, p2);
					float beta = ComputeFunction(pixelCenter.X, pixelCenter.Y, p2, p0) / ComputeFunction(p1.X, p1.Y, p2, p0);
					float gamma = ComputeFunction(pixelCenter.X, pixelCenter.Y, p0, p1) / ComputeFunction(p2.X, p2.Y, p0, p1);

					// Create output fragment.
					var fragment = new Fragment(x, y);
					fragment.PixelShaderInput = CreatePixelShaderInput(triangle, beta, alpha, gamma);
					fragment.Samples = samples;

					// TODO: Is this needed? We already have the depths for each sample.
					fragment.Depth = FloatInterpolator.InterpolateLinear(alpha, beta, gamma, p0.Z, p1.Z, p2.Z);

					outputs.Add(fragment);
				}
		}

		private object CreatePixelShaderInput(TrianglePrimitive triangle, float beta, float alpha, float gamma)
		{
			var pixelShaderInput = _pixelShaderStage.BuildPixelShaderInput();

			// TODO: Use Cache API
			// Calculate interpolated attribute values for this fragment.
			var vertexShaderDescription = ShaderDescriptionCache.GetDescription(_vertexShaderStage.VertexShader);
			var pixelShaderDescription = ShaderDescriptionCache.GetDescription(_pixelShaderStage.PixelShader);
			foreach (var property in pixelShaderDescription.InputParameters)
			{
				// Grab values from vertex shader outputs.
				var outputProperty = vertexShaderDescription.GetOutputParameterBySemantic(property.Semantic);
				object v1Value = outputProperty.GetValue(triangle.V1);
				object v2Value = outputProperty.GetValue(triangle.V2);
				object v3Value = outputProperty.GetValue(triangle.V3);

				// Interpolate values.
				// TODO: Use attribute to indicate whether perspective or linear interpolation is required.
				object interpolatedValue = Interpolator.Perspective(alpha, beta, gamma, v1Value, v2Value, v3Value,
					triangle.V1.Position.W, triangle.V2.Position.W, triangle.V3.Position.W);
				//object interpolatedValue = Interpolator.Linear(alpha, beta, gamma, v1Value, v2Value, v3Value);

				// Set value onto pixel shader input.
				property.SetValue(ref pixelShaderInput, interpolatedValue);

				// TODO: Do something different if input parameter is a system value.
			}
			return pixelShaderInput;
		}

		private bool CalculateSampleCoverage(int x, int y, Point4D p0, Point4D p2, Point4D p1, SampleCollection samples)
		{
			bool anyCoveredSamples = false;
			for (int sampleIndex = 0; sampleIndex < _outputMerger.RenderTarget.MultiSampleCount; ++sampleIndex)
			{
				// Is this pixel inside triangle?
				Point2D samplePosition = GetSamplePosition(x, y, sampleIndex);

				float depth;
				bool covered = IsSampleInsideTriangle(p0, p1, p2, samplePosition, out depth);
				samples.Add(new Sample
				{
					Covered = covered,
					Depth = depth
				});
				if (covered)
					anyCoveredSamples = true;
			}
			return anyCoveredSamples;
		}

		private static float ComputeFunction(float x, float y, Point4D pa, Point4D pb)
		{
			return (pa.Y - pb.Y) * x + (pb.X - pa.X) * y + pa.X * pb.Y - pb.X * pa.Y;
		}

		private bool IsSampleInsideTriangle(Point4D p0, Point4D p1, Point4D p2, Point2D samplePosition, out float depth)
		{
			// TODO: Use fill convention.

			// Calculate alpha, beta, gamma for this sample position.
			float alpha = ComputeFunction(samplePosition.X, samplePosition.Y, p1, p2) / ComputeFunction(p0.X, p0.Y, p1, p2);
			float beta = ComputeFunction(samplePosition.X, samplePosition.Y, p2, p0) / ComputeFunction(p1.X, p1.Y, p2, p0);
			float gamma = ComputeFunction(samplePosition.X, samplePosition.Y, p0, p1) / ComputeFunction(p2.X, p2.Y, p0, p1);

			// Calculate depth.
			// TODO: Does this only need to be calculated if the sample is inside the triangle?
			depth = FloatInterpolator.InterpolateLinear(alpha, beta, gamma, p0.Z, p1.Z, p2.Z);

			// If any of these tests fails, the current pixel is not inside the triangle.
			// TODO: Only need to test if > 1?
			if (alpha < 0 || alpha > 1 || beta < 0 || beta > 1 || gamma < 0 || gamma > 1)
			{
				depth = 0;
				return false;
			}

			// The exact value to compare against depends on fill mode - if we're rendering wireframe,
			// then check whether sample position is within the "wireframe threshold" (i.e. 1 pixel) of an edge.
			switch (FillMode)
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
}