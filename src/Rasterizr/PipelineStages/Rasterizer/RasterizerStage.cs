using System;
using System.Collections.Generic;
using Nexus;
using Rasterizr.PipelineStages.OutputMerger;
using Rasterizr.PipelineStages.Rasterizer.Interpolation;
using Rasterizr.PipelineStages.ShaderStages.Core;
using Rasterizr.PipelineStages.ShaderStages.GeometryShader;
using Rasterizr.PipelineStages.ShaderStages.PixelShader;
using Rasterizr.PipelineStages.ShaderStages.VertexShader;

namespace Rasterizr.PipelineStages.Rasterizer
{
	public class RasterizerStage : PipelineStageBase<IVertexShaderOutput, Fragment>
	{
		private readonly Viewport3D _viewport;
		private readonly PixelShaderStage _pixelShaderStage;
		private readonly OutputMergerStage _outputMerger;
		private readonly Fragment[,] _fragments;

		public CullMode CullMode { get; set; }
		public FillMode FillMode { get; set; }
		public bool MultiSampleAntiAlias { get; set; }

		public RasterizerStage(Viewport3D viewport, PixelShaderStage pixelShaderStage, OutputMergerStage outputMerger)
		{
			_viewport = viewport;
			_pixelShaderStage = pixelShaderStage;
			_outputMerger = outputMerger;

			_fragments = new Fragment[viewport.Width, viewport.Height];
			for (int y = 0; y < viewport.Height; ++y)
				for (int x = 0; x < viewport.Width; ++x)
					_fragments[x, y] = new Fragment(x, y);

			CullMode = CullMode.CullCounterClockwiseFace;
			FillMode = FillMode.Solid;
		}

		public override void Process(IList<IVertexShaderOutput> inputs, IList<Fragment> outputs)
		{
			// TODO: Clipping and backface culling.

			// Transform to screen space.
			for (int i = 0; i < inputs.Count; i++)
				inputs[i].Position = ToScreenCoordinates(inputs[i].Position);

			// Rasterize.
			for (int i = 0; i < inputs.Count; i += 3)
			{
				var triangle = new TrianglePrimitive(inputs[i + 0], inputs[i + 1], inputs[i + 2]);

				// Determine screen bounds of triangle so we know which pixels to test.
				Box2D screenBounds = GetScreenBounds(triangle);

				// Scan pixels in target area, checking if they are inside the triangle.
				// If they are, calculate the coverage.
				ScanSamples(screenBounds, outputs, triangle);
			}
		}

		private Point4D ToScreenCoordinates(Point4D position)
		{
			// pra [0,1]
			position.X = (position.X + 1f) / 2.0f;
			position.Y = (position.Y + 1f) / 2.0f;

			if (position.X > 1 || position.Y > 1 || position.X < 0 || position.Y < 0)
			{
				// we got a situation
				throw new Exception("BLA");
			}

			if (position.Z < 0)
			{
				// we got a situation also
				throw new Exception("BLA");
			}

			// pra coordenadas de tela
			position.X *= (_viewport.Width - 1);
			position.Y *= (_viewport.Height - 1);
			return position;
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

		private void ScanSamples(Box2D screenBounds, IList<Fragment> outputs, TrianglePrimitive triangle)
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
					SampleCollection samples = new SampleCollection();
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

					if (anyCoveredSamples)
					{
						Point2D pixelCenter = new Point2D(x + 0.5f, y + 0.5f);

						// Calculate alpha, beta, gamma for pixel center.
						float alpha = ComputeFunction(pixelCenter.X, pixelCenter.Y, p1, p2) / ComputeFunction(p0.X, p0.Y, p1, p2);
						float beta = ComputeFunction(pixelCenter.X, pixelCenter.Y, p2, p0) / ComputeFunction(p1.X, p1.Y, p2, p0);
						float gamma = ComputeFunction(pixelCenter.X, pixelCenter.Y, p0, p1) / ComputeFunction(p2.X, p2.Y, p0, p1);

						// Create output fragment.
						Fragment fragment = new Fragment(x, y);
						var pixelShaderInput = _pixelShaderStage.BuildPixelShaderInput();

						// TODO: Use Cache API
						// Calculate interpolated attribute values for this fragment.
						var vertexShaderOutputDescription = new ShaderInputOutputDescription(triangle.V1.GetType());
						var pixelShaderInputDescription = new ShaderInputOutputDescription(pixelShaderInput.GetType());
						foreach (var property in pixelShaderInputDescription.Properties)
						{
							// Grab values from vertex shader outputs.
							object v1Value = vertexShaderOutputDescription.GetValue(triangle.V1, property.Semantic);
							object v2Value = vertexShaderOutputDescription.GetValue(triangle.V2, property.Semantic);
							object v3Value = vertexShaderOutputDescription.GetValue(triangle.V3, property.Semantic);

							// Interpolate values.
							// TODO: Use attribute to indicate whether perspective or linear interpolation is required.
							object interpolatedValue = Interpolator.Perspective(alpha, beta, gamma, v1Value, v2Value, v3Value,
								triangle.V1.Position.W, triangle.V2.Position.W, triangle.V3.Position.W);

							// Set value onto pixel shader input.
							pixelShaderInputDescription.SetValue(pixelShaderInput, property.Semantic, interpolatedValue);
						}

						// TODO: Is this needed? We already have the depths for each sample.
						fragment.Depth = FloatInterpolator.InterpolateLinear(alpha, beta, gamma, p0.Z, p1.Z, p2.Z);

						outputs.Add(fragment);
					}
			}
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