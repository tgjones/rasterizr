using System;
using System.Collections.Generic;
using System.Linq;
using Nexus;
using Rasterizr.PipelineStages.OutputMerger;
using Rasterizr.PipelineStages.ShaderStages.Core;
using Rasterizr.PipelineStages.ShaderStages.GeometryShader;
using Rasterizr.PipelineStages.TriangleSetup;
using Rasterizr.VertexAttributes;

namespace Rasterizr.PipelineStages.Rasterizer
{
	public class RasterizerStage : PipelineStageBase<TrianglePrimitive, Fragment>
	{
		private readonly Viewport3D _viewport;
		private readonly OutputMergerStage _outputMerger;
		private readonly Fragment[,] _fragments;

		public CullMode CullMode { get; set; }
		public FillMode FillMode { get; set; }
		public bool MultiSampleAntiAlias { get; set; }

		public RasterizerStage(Viewport3D viewport, OutputMergerStage outputMerger)
		{
			_viewport = viewport;
			_outputMerger = outputMerger;

			_fragments = new Fragment[viewport.Width, viewport.Height];
			for (int y = 0; y < viewport.Height; ++y)
				for (int x = 0; x < viewport.Width; ++x)
					_fragments[x, y] = new Fragment(x, y);

			CullMode = CullMode.CullCounterClockwiseFace;
			FillMode = FillMode.Solid;
		}

		public override void Process(IList<TrianglePrimitive> inputs, IList<Fragment> outputs)
		{
			foreach (TrianglePrimitive triangle2 in inputs)
			{
				// Transform from clip-space to homogeneous screen space.
				TrianglePrimitive triangle = triangle2;
				TransformFromClipSpaceToScreenSpace(ref triangle);

				// Setup edge edquations for triangle.
					// TODO: Might need to transpose matrix.
				Matrix2D vertexMatrix = new Matrix2D
				{
					M11 = triangle.V1.Position.X,
					M12 = triangle.V2.Position.X,
					M13 = triangle.V3.Position.X,
					M21 = triangle.V1.Position.Y,
					M22 = triangle.V2.Position.Y,
					M23 = triangle.V3.Position.Y,
					M31 = triangle.V1.Position.W,
					M32 = triangle.V2.Position.W,
					M33 = triangle.V3.Position.W
				};
				float determinant = vertexMatrix.Determinant;

				// The determinant tells us some useful things - if it's 0, either the triangle is degenerate
				// or the view is edge-on. If it's positive, the triangle is front-facing.
				if (MathUtility.IsZero(determinant))
					continue;
				if (CullingUtility.ShouldCull(determinant, CullMode))
					continue;

				Matrix2D vertexMatrixInverse = Matrix2D.Adjoint(vertexMatrix) / determinant;
				TriangleSetupInfo triangleSetup = new TriangleSetupInfo
				{
					VertexMatrixInverse = vertexMatrixInverse,
					ZInterpolator = InterpolatorUtility.CreateInterpolator(vertexMatrixInverse, triangle.V1.Position.Z, triangle.V2.Position.Z, triangle.V3.Position.Z),
					//WInterpolator = InterpolatorUtility.CreateInterpolator(vertexMatrixInverse, triangle.V1.Position.W, triangle.V2.Position.W, triangle.V3.Position.W),
					ConstantInterpolator = InterpolatorUtility.CreateInterpolator(vertexMatrixInverse, 1, 1, 1),
					AttributeInterpolators = GetAttributeInterpolators(vertexMatrixInverse, triangle),
					EdgeFunction1 = InterpolatorUtility.CreateInterpolator(vertexMatrixInverse, 1, 0, 0),
					EdgeFunction2 = InterpolatorUtility.CreateInterpolator(vertexMatrixInverse, 0, 1, 0),
					EdgeFunction3 = InterpolatorUtility.CreateInterpolator(vertexMatrixInverse, 0, 0, 1)
				};

				// Determine screen bounds of triangle so we know which pixels to test.
				Box2D screenBounds = GetScreenBounds(triangle);

				// Scan pixels in target area, checking if they are inside the triangle.
				// If they are, calculate the coverage.
				ScanSamples(screenBounds, triangleSetup, outputs, triangle);
			}
		}

		private static List<Interpolator> GetAttributeInterpolators(Matrix2D vertexMatrixInverse, TrianglePrimitive triangle)
		{
			List<Interpolator> interpolators = new List<Interpolator>();
			for (int i = 0; i < triangle.V1.Attributes.Count; ++i)
				interpolators.Add(InterpolatorUtility.CreateInterpolator(vertexMatrixInverse,
					triangle.V1.Attributes[i].Value,
					triangle.V2.Attributes[i].Value,
					triangle.V3.Attributes[i].Value));
			return interpolators;
		}

		private void TransformFromClipSpaceToScreenSpace(ref TrianglePrimitive primitive)
		{
			int screenWidthDiv2 = _viewport.Width / 2;
			int screenHeightDiv2 = _viewport.Height / 2;
			const int offsetX = 0;
			const int offsetY = 0;

			Matrix3D viewportScaleMatrix = Matrix3D.CreateScale(screenWidthDiv2, -screenHeightDiv2, 0.5f)
				* Matrix3D.CreateTranslation(screenWidthDiv2 + offsetX, screenHeightDiv2 + offsetY, 0.5f);

			primitive.V1.Position = viewportScaleMatrix.Transform(primitive.V1.Position);
			primitive.V2.Position = viewportScaleMatrix.Transform(primitive.V2.Position);
			primitive.V3.Position = viewportScaleMatrix.Transform(primitive.V3.Position);
		}

		private Box2D GetScreenBounds(TrianglePrimitive triangle)
		{
			// TODO: Use Blinn's method for determining screen bounding box of triangle.

			Point3D p1 = triangle.V1.Position.ToPoint3D();
			Point3D p2 = triangle.V2.Position.ToPoint3D();
			Point3D p3 = triangle.V3.Position.ToPoint3D();

			Point3D[] points = new [] { p1, p2, p3 };
			int minX = MathUtility.Floor(points.Min(p => p.X));
			int maxX = MathUtility.Ceiling(points.Max(p => p.X));
			int minY = MathUtility.Floor(points.Min(p => p.Y));
			int maxY = MathUtility.Ceiling(points.Max(p => p.Y));

			Box2D bounds = new Box2D(new IntPoint2D(minX, minY), new IntPoint2D(maxX, maxY));

			// Clip bounds to viewport.
			bounds = Box2D.Intersection(bounds, new Box2D(new IntPoint2D(0, 0), new IntPoint2D(_viewport.Width, _viewport.Height)));

			return bounds;
		}

		public Point2D GetSamplePosition(int x, int y, int sampleIndex)
		{
			return MultiSamplingUtility.GetSamplePosition(
				_outputMerger.RenderTarget.MultiSampleCount,
				x, y, sampleIndex);
		}

		private void ScanSamples(Box2D screenBounds, TriangleSetupInfo triangleSetup, IList<Fragment> outputs,
			TrianglePrimitive triangle)
		{
			// TODO: Parallelize this?
			for (int y = screenBounds.Min.Y; y < screenBounds.Max.Y; ++y)
				for (int x = screenBounds.Min.X; x < screenBounds.Max.X; ++x)
				{
					// Check all samples to determine whether they are inside the triangle.
					SampleCollection samples = new SampleCollection();
					bool anyCoveredSamples = false;
					for (int sampleIndex = 0; sampleIndex < _outputMerger.RenderTarget.MultiSampleCount; ++sampleIndex)
					{
						// Is this pixel inside triangle?
						Point2D samplePosition = GetSamplePosition(x, y, sampleIndex);

						bool covered = IsSampleInsideTriangle(triangleSetup, samplePosition);
						samples.Add(new Sample
						{
							Covered = covered,
							Depth = GetNonPerspectiveCorrectInterpolatedValue(triangleSetup.ZInterpolator, samplePosition).Value
						});
						if (covered)
							anyCoveredSamples = true;
					}

					if (anyCoveredSamples)
					{
						Point2D pixelCenter = new Point2D(x + 0.5f, y + 0.5f);
						// Calculate interpolated attribute values for this fragment.
						float z = GetNonPerspectiveCorrectInterpolatedValue(triangleSetup.ZInterpolator, pixelCenter).Value;
						//float z = GetNonPerspectiveCorrectInterpolatedValue(triangleSetup.ConstantInterpolator, pixelCenter, triangleSetup.ConstantInterpolator).Value;
						AddFragmentToOutput(x, y, z, outputs, triangleSetup, triangle, pixelCenter, samples);
					}
				}
		}

		private static IVertexAttributeValue GetPerspectiveCorrectInterpolatedValue(Interpolator interpolator, Point2D samplePosition, Interpolator<FloatVertexAttributeValue> constantInterpolator)
		{
			IVertexAttributeValue value = interpolator.GetInterpolatedValue(samplePosition);
			float oneOverW = constantInterpolator.GetInterpolatedValue(samplePosition).Value;

			return value.Multiply(1 / oneOverW);
		}

		private static FloatVertexAttributeValue GetNonPerspectiveCorrectInterpolatedValue(Interpolator<FloatVertexAttributeValue> interpolator, Point2D samplePosition)
		{
			IVertexAttributeValue value = interpolator.GetInterpolatedValue(samplePosition);
			return (FloatVertexAttributeValue)value;
		}

		private bool IsSampleInsideTriangle(TriangleSetupInfo triangle, Point2D samplePosition)
		{
			// TODO: Use fill convention.

			// Test whether sample position is on the positive side of the three edges.
			float edgeValue1 = triangle.EdgeFunction1.GetInterpolatedValue(samplePosition).Value;
			float edgeValue2 = triangle.EdgeFunction2.GetInterpolatedValue(samplePosition).Value;
			float edgeValue3 = triangle.EdgeFunction3.GetInterpolatedValue(samplePosition).Value;

			const float comparison = 0;
			if (!(edgeValue1 > comparison && edgeValue2 > comparison && edgeValue3 > comparison))
				return false;

			// The exact value to compare against depends on fill mode - if we're rendering wireframe,
			// then check whether sample position is within the "wireframe threshold" (i.e. 1 pixel) of an edge.
			switch (FillMode)
			{
				case FillMode.Solid :
					return true;
				case FillMode.Wireframe :
					const float wireframeThreshold = 0.00001f;
					return edgeValue1 < wireframeThreshold || edgeValue2 < wireframeThreshold || edgeValue3 < wireframeThreshold;
				default :
					throw new NotSupportedException();
			}
		}

		private void AddFragmentToOutput(int x, int y, float z, IList<Fragment> outputs, TriangleSetupInfo triangleSetup, TrianglePrimitive triangle, Point2D samplePosition, SampleCollection samples)
		{
			Fragment fragment = _fragments[x, y];
			fragment.Samples = samples;
			fragment.Attributes.ActiveLength = triangle.V1.Attributes.Count;

			for (int i = 0; i < triangle.V1.Attributes.Count; ++i)
			{
				// Calculate interpolated value.
				IVertexAttributeValue value = GetPerspectiveCorrectInterpolatedValue(triangleSetup.AttributeInterpolators[i], samplePosition, triangleSetup.ConstantInterpolator);

				fragment.Attributes[i] = new InterpolatedVertexAttribute
				{
					Name = triangle.V1.Attributes[i].Name,
					Semantic = triangle.V1.Attributes[i].Semantic,
					Value = value,
					DValueDx = value, // TODO
					DValueDy = value // TODO
				};
			}

			fragment.Attributes[triangle.V1.Attributes.Count] = new InterpolatedVertexAttribute
			{
				Name = Semantics.SV_Depth,
				Semantic = Semantics.SV_Depth,
				Value = new FloatVertexAttributeValue { Value = z },
				DValueDx = new FloatVertexAttributeValue { Value = z }, // TODO
				DValueDy = new FloatVertexAttributeValue { Value = z } // TODO
			};
			++fragment.Attributes.ActiveLength;

			outputs.Add(fragment);
		}
	}
}