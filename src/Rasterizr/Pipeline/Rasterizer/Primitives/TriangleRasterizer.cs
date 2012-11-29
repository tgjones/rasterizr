using System;
using System.Collections.Generic;
using Rasterizr.Math;
using Rasterizr.Pipeline.InputAssembler;

namespace Rasterizr.Pipeline.Rasterizer.Primitives
{
	internal class TriangleRasterizer : PrimitiveRasterizer
	{
		private InputAssemblerPrimitiveOutput _primitive;
		private Vector4 _p0, _p1, _p2;

		private float _alphaDenominator;
		private float _betaDenominator;
		private float _gammaDenominator;

		public override InputAssemblerPrimitiveOutput Primitive
		{
			get { return _primitive; }
			set
			{
				_primitive = value;
				_p0 = _primitive.Vertices[0].Position;
				_p1 = _primitive.Vertices[1].Position;
				_p2 = _primitive.Vertices[2].Position;
			}
		}

		public override IEnumerable<FragmentQuad> Rasterize()
		{
			// Precompute alpha, beta and gamma denominator values. These are the same for all fragments.
			_alphaDenominator = ComputeFunction(_p0.X, _p0.Y, ref _p1, ref _p2);
			_betaDenominator = ComputeFunction(_p1.X, _p1.Y, ref _p2, ref _p0);
			_gammaDenominator = ComputeFunction(_p2.X, _p2.Y, ref _p0, ref _p1);

			var screenBounds = Box2D.CreateBoundingBox(ref _p0, ref _p1, ref _p2);

			// Scan pixels in target area, checking if they are inside the triangle.
			// If they are, calculate the coverage.

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
					bool anyCoveredSamples = CalculateSampleCoverage(ref fragmentQuad.Fragment0);
					anyCoveredSamples = anyCoveredSamples || CalculateSampleCoverage(ref fragmentQuad.Fragment1);
					anyCoveredSamples = anyCoveredSamples || CalculateSampleCoverage(ref fragmentQuad.Fragment2);
					anyCoveredSamples = anyCoveredSamples || CalculateSampleCoverage(ref fragmentQuad.Fragment3);
					if (!anyCoveredSamples)
						continue;

					// Otherwise, we do have at least one fragment with covered samples, so continue
					// with interpolation. We need to interpolate values for all fragments in this quad,
					// even though they may not all be covered, because we need all four fragments in order
					// to calculate derivatives correctly.
					InterpolateFragmentData(ref fragmentQuad.Fragment0);
					InterpolateFragmentData(ref fragmentQuad.Fragment1);
					InterpolateFragmentData(ref fragmentQuad.Fragment2);
					InterpolateFragmentData(ref fragmentQuad.Fragment3);

					yield return fragmentQuad;
				}
		}

		private static int NearestEvenNumber(int value)
		{
			return (value % 2 == 0) ? value : value - 1;
		}

		private static float ComputeFunction(float x, float y, ref Vector4 pa, ref Vector4 pb)
		{
			return (pa.Y - pb.Y) * x + (pb.X - pa.X) * y + pa.X * pb.Y - pb.X * pa.Y;
		}

		private void InterpolateFragmentData(ref Fragment fragment)
		{
			var pixelCenter = new Vector2(fragment.X + 0.5f, fragment.Y + 0.5f);

			// Calculate alpha, beta, gamma for pixel center.
			float alpha = ComputeFunction(pixelCenter.X, pixelCenter.Y, ref _p1, ref _p2)
				/ ComputeFunction(_p0.X, _p0.Y, ref _p1, ref _p2);
			float beta = ComputeFunction(pixelCenter.X, pixelCenter.Y, ref _p2, ref _p0)
				/ ComputeFunction(_p1.X, _p1.Y, ref _p2, ref _p0);
			float gamma = ComputeFunction(pixelCenter.X, pixelCenter.Y, ref _p0, ref _p1)
				/ ComputeFunction(_p2.X, _p2.Y, ref _p0, ref _p1);

			// Create pixel shader input.
			fragment.Data = CreatePixelShaderInput(beta, alpha, gamma);
		}

		private bool CalculateSampleCoverage(ref Fragment fragment)
		{
			int multiSampleCount = MultiSampleCount;
			bool anyCoveredSamples = false;
			if (multiSampleCount > 0)
				anyCoveredSamples = CalculateSampleCoverage(ref fragment, 0, out fragment.Samples.Sample0);
			if (multiSampleCount > 1)
				anyCoveredSamples = anyCoveredSamples || CalculateSampleCoverage(ref fragment, 1, out fragment.Samples.Sample1);
			if (multiSampleCount > 2)
				anyCoveredSamples = anyCoveredSamples || CalculateSampleCoverage(ref fragment, 2, out fragment.Samples.Sample2);
			if (multiSampleCount > 3)
				anyCoveredSamples = anyCoveredSamples || CalculateSampleCoverage(ref fragment, 3, out fragment.Samples.Sample3);
			fragment.Samples.AnyCovered = anyCoveredSamples;
			return anyCoveredSamples;
		}

		private bool CalculateSampleCoverage(ref Fragment fragment, int sampleIndex, out Sample sample)
		{
			// Is this pixel inside triangle?
			Vector2 samplePosition = GetSamplePosition(fragment.X, fragment.Y, sampleIndex);

			float depth;
			bool covered = IsSampleInsideTriangle(ref samplePosition, out depth);
			sample = new Sample
			{
				Covered = covered,
				Depth = depth
			};
			return covered;
		}

		private bool IsSampleInsideTriangle(ref Vector2 samplePosition, out float depth)
		{
			// TODO: Use fill convention.

			// Calculate alpha, beta, gamma for this sample position.
			float alpha = ComputeFunction(samplePosition.X, samplePosition.Y, ref _p1, ref _p2) / _alphaDenominator;
			float beta = ComputeFunction(samplePosition.X, samplePosition.Y, ref _p2, ref _p0) / _betaDenominator;
			float gamma = ComputeFunction(samplePosition.X, samplePosition.Y, ref _p0, ref _p1) / _gammaDenominator;

			// Calculate depth.
			// TODO: Does this only need to be calculated if the sample is inside the triangle?
			depth = InterpolationUtility.Linear(alpha, beta, gamma, _p0.Z, _p1.Z, _p2.Z);

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

		private Vector4[] CreatePixelShaderInput(float beta, float alpha, float gamma)
		{
			// TODO: Cache as much of this as possible.
			// Calculate interpolated attribute values for this fragment.
			var result = new Vector4[PixelShaderInputSignture.Parameters.Count];
			for (int i = 0; i < PixelShaderInputSignture.Parameters.Count; i++)
			{
				var parameter = PixelShaderInputSignture.Parameters[i];

				// Grab values from vertex shader outputs.
				var outputParameterRegister = PreviousStageOutputSignature.Parameters.FindRegister(
					parameter.SemanticName, parameter.SemanticIndex);

				var v0Value = _primitive.Vertices[0].Data[outputParameterRegister];
				var v1Value = _primitive.Vertices[1].Data[outputParameterRegister];
				var v2Value = _primitive.Vertices[2].Data[outputParameterRegister];

				// Interpolate values.
				const bool isPerspectiveCorrect = true; // TODO
				Vector4 interpolatedValue = (isPerspectiveCorrect)
					? InterpolationUtility.Perspective(alpha, beta, gamma, ref v0Value, ref v1Value, ref v2Value,
						_p0.W, _p1.W, _p2.W)
					: InterpolationUtility.Linear(alpha, beta, gamma, ref v0Value, ref v1Value, ref v2Value);

				// Set value onto pixel shader input.
				result[i] = interpolatedValue;

				// TODO: Do something different if input parameter is a system value.
			}
			return result;
		}
	}
}