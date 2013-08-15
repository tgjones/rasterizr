using System;
using System.Collections.Generic;
using Rasterizr.Math;
using Rasterizr.Pipeline.InputAssembler;
using SlimShader;
using SlimShader.Chunks.Shex;

namespace Rasterizr.Pipeline.Rasterizer.Primitives
{
	internal class TriangleRasterizer : PrimitiveRasterizer
	{
		private InputAssemblerPrimitiveOutput _primitive;
		private Number4 _p0, _p1, _p2;

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

        public override bool ShouldCull(
            VertexShader.VertexShaderOutput[] vertices)
        {
            var a = vertices[0].Position;
            var b = vertices[1].Position;
            var c = vertices[2].Position;

            var ab = Number4.Subtract(ref b, ref a);
            var ac = Number4.Subtract(ref c, ref a);

            var l0 = new Vector3ForCulling(ab.X, ab.Y, ab.Z);
            var l1 = new Vector3ForCulling(ac.X, ac.Y, ac.Z);

            l0.Normalize();
            l1.Normalize();

            var normal = Vector3ForCulling.CrossZ(ref l0, ref l1);

            return RasterizerState.ShouldCull(normal > 0);
        }

        private struct Vector3ForCulling
        {
            public float X;
            public float Y;
            public float Z;

            public Vector3ForCulling(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            /// <summary>
            /// Converts the vector into a unit vector.
            /// </summary>
            public void Normalize()
            {
                var length = (float) System.Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
                var inv = 1.0f / length;
                X *= inv;
                Y *= inv;
                Z *= inv;
            }

            /// <summary>
            /// Calculates the cross-product, but only of the Z component,
            /// because that's all we need in order to do backface culling.
            /// </summary>
            public static float CrossZ(ref Vector3ForCulling left, ref Vector3ForCulling right)
            {
                return (left.X * right.Y) - (left.Y * right.X);
            }
        }

		public override IEnumerable<FragmentQuad> Rasterize()
		{
			// Precompute alpha, beta and gamma denominator values. These are the same for all fragments.
			_alphaDenominator = ComputeFunction(_p0.X, _p0.Y, ref _p1, ref _p2);
			_betaDenominator = ComputeFunction(_p1.X, _p1.Y, ref _p2, ref _p0);
			_gammaDenominator = ComputeFunction(_p2.X, _p2.Y, ref _p0, ref _p1);

            // TODO: Handle degenerate triangles. The following code might be right, but need to check.
		    //const float degenerateThreshold = 0.001f;
            //if (_alphaDenominator < degenerateThreshold || _betaDenominator < degenerateThreshold || _gammaDenominator < degenerateThreshold)
            //    yield break;

			var screenBounds = Box2D.CreateBoundingBox(ref _p0, ref _p1, ref _p2);

            // Clip triangle's screen bounds to viewport bounds.
            // TODO: Is this really how clipping works? How about "hither"
            // vertices very close to the eye, is there a problem with homogeneous
            // coordinates?
		    screenBounds.MinX = System.Math.Max(screenBounds.MinX, 0);
            screenBounds.MaxX = System.Math.Min(screenBounds.MaxX, Viewport.TopLeftX + Viewport.Width - 1);
            screenBounds.MinY = System.Math.Max(screenBounds.MinY, 0);
            screenBounds.MaxY = System.Math.Min(screenBounds.MaxY, Viewport.TopLeftY + Viewport.Height - 1);

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
					// need to do any (expensive) interpolation of attributes.
					var fragmentQuad = new FragmentQuad
					{
						Fragment0 = new Fragment(_primitive.PrimitiveID, x, y, FragmentQuadLocation.TopLeft),
						Fragment1 = new Fragment(_primitive.PrimitiveID, x + 1, y, FragmentQuadLocation.TopRight),
						Fragment2 = new Fragment(_primitive.PrimitiveID, x, y + 1, FragmentQuadLocation.BottomLeft),
						Fragment3 = new Fragment(_primitive.PrimitiveID, x + 1, y + 1, FragmentQuadLocation.BottomRight)
					};

					if (IsMultiSamplingEnabled)
					{
						// For multisampling, we test coverage and interpolate attributes in two separate steps.
						// Check all samples to determine whether they are inside the triangle.
						bool covered0 = CalculateSampleCoverage(ref fragmentQuad.Fragment0);
						bool covered1 = CalculateSampleCoverage(ref fragmentQuad.Fragment1);
						bool covered2 = CalculateSampleCoverage(ref fragmentQuad.Fragment2);
						bool covered3 = CalculateSampleCoverage(ref fragmentQuad.Fragment3);
						if (!covered0 && !covered1 && !covered2 && !covered3)
							continue;

						// Otherwise, we do have at least one fragment with covered samples, so continue
						// with interpolation. We need to interpolate values for all fragments in this quad,
						// even though they may not all be covered, because we need all four fragments in order
						// to calculate derivatives correctly.
						InterpolateFragmentData(ref fragmentQuad.Fragment0);
						InterpolateFragmentData(ref fragmentQuad.Fragment1);
						InterpolateFragmentData(ref fragmentQuad.Fragment2);
						InterpolateFragmentData(ref fragmentQuad.Fragment3);
					}
					else
					{
						BarycentricCoordinates fragment0Coordinates, fragment1Coordinates, fragment2Coordinates, fragment3Coordinates;

						// For non-multisampling, we can re-use the same calculations for coverage and interpolation.
						bool covered0 = CalculateCoverageAndInterpolateFragmentData(ref fragmentQuad.Fragment0, out fragment0Coordinates);
						bool covered1 = CalculateCoverageAndInterpolateFragmentData(ref fragmentQuad.Fragment1, out fragment1Coordinates);
						bool covered2 = CalculateCoverageAndInterpolateFragmentData(ref fragmentQuad.Fragment2, out fragment2Coordinates);
						bool covered3 = CalculateCoverageAndInterpolateFragmentData(ref fragmentQuad.Fragment3, out fragment3Coordinates);

						if (!covered0 && !covered1 && !covered2 && !covered3)
							continue;

						// Create pixel shader input.
						fragmentQuad.Fragment0.Data = CreatePixelShaderInput(ref fragment0Coordinates);
						fragmentQuad.Fragment1.Data = CreatePixelShaderInput(ref fragment1Coordinates);
						fragmentQuad.Fragment2.Data = CreatePixelShaderInput(ref fragment2Coordinates);
						fragmentQuad.Fragment3.Data = CreatePixelShaderInput(ref fragment3Coordinates);
					}

					yield return fragmentQuad;
				}
		}

		private bool CalculateCoverageAndInterpolateFragmentData(ref Fragment fragment, out BarycentricCoordinates coordinates)
		{
			// For non-multisampling, we can re-use the same calculations for coverage and interpolation.
            var pixelCenter = new Point(fragment.X + 0.5f, fragment.Y + 0.5f);
			CalculateBarycentricCoordinates(ref pixelCenter, out coordinates);

			float depth;
			if (!IsSampleInsideTriangle(ref coordinates, out depth))
				return false;

			fragment.Samples.Sample0.Covered = true;
			fragment.Samples.Sample0.Depth = depth;
			fragment.Samples.AnyCovered = true;

			return true;
		}

		private static int NearestEvenNumber(int value)
		{
			return (value % 2 == 0) ? value : value - 1;
		}

		private static float ComputeFunction(float x, float y, ref Number4 pa, ref Number4 pb)
		{
			return (pa.Y - pb.Y) * x + (pb.X - pa.X) * y + pa.X * pb.Y - pb.X * pa.Y;
		}

		private void InterpolateFragmentData(ref Fragment fragment)
		{
            var pixelCenter = new Point(fragment.X + 0.5f, fragment.Y + 0.5f);

			// Calculate alpha, beta, gamma for pixel center.
			BarycentricCoordinates coordinates;
			CalculateBarycentricCoordinates(ref pixelCenter, out coordinates);

			// Create pixel shader input.
			fragment.Data = CreatePixelShaderInput(ref coordinates);
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
			var samplePosition = GetSamplePosition(fragment.X, fragment.Y, sampleIndex);

			float depth;
			bool covered = IsSampleInsideTriangle(ref samplePosition, out depth);
			sample = new Sample
			{
				Covered = covered,
				Depth = depth
			};
			return covered;
		}

		private bool IsSampleInsideTriangle(ref BarycentricCoordinates coordinates, out float depth)
		{
			// TODO: Use fill convention.

			// Calculate depth.
			// TODO: Does this only need to be calculated if the sample is inside the triangle?
			depth = InterpolationUtility.Linear(coordinates.Alpha, coordinates.Beta, coordinates.Gamma, _p0.Z, _p1.Z, _p2.Z);

			// If any of these tests fails, the current pixel is not inside the triangle.
			if (coordinates.IsOutsideTriangle)
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
					const float wireframeThreshold = 0.1f;
					return coordinates.Alpha < wireframeThreshold || coordinates.Beta < wireframeThreshold || coordinates.Gamma < wireframeThreshold;
				default:
					throw new NotSupportedException();
			}
		}

        private bool IsSampleInsideTriangle(ref Point samplePosition, out float depth)
		{
			BarycentricCoordinates coordinates;
			CalculateBarycentricCoordinates(ref samplePosition, out coordinates);
			return IsSampleInsideTriangle(ref coordinates, out depth);
		}

        private void CalculateBarycentricCoordinates(ref Point position, out BarycentricCoordinates coordinates)
		{
			// Calculate alpha, beta, gamma for pixel center.
			coordinates.Alpha = ComputeFunction(position.X, position.Y, ref _p1, ref _p2) / _alphaDenominator;
			coordinates.Beta = ComputeFunction(position.X, position.Y, ref _p2, ref _p0) / _betaDenominator;
			coordinates.Gamma = ComputeFunction(position.X, position.Y, ref _p0, ref _p1) / _gammaDenominator;
		}

		private Number4[] CreatePixelShaderInput(ref BarycentricCoordinates coordinates)
		{
			float w = InterpolationUtility.PrecalculateW(
				coordinates.Alpha, coordinates.Beta, coordinates.Gamma,
				_p0.W, _p1.W, _p2.W);

			// TODO: Cache as much of this as possible.
			// Calculate interpolated attribute values for this fragment.
            var result = new Number4[OutputInputRegisterMappings.Length];
		    for (int i = 0; i < result.Length; i++)
		    {
		        var outputRegister = OutputInputRegisterMappings[i];
		        var v0Value = _primitive.Vertices[0].Data[outputRegister];
		        var v1Value = _primitive.Vertices[1].Data[outputRegister];
		        var v2Value = _primitive.Vertices[2].Data[outputRegister];

		        // Interpolate values.
		        Number4 interpolatedValue;
		        switch (InputRegisterInterpolationModes[i])
		        {
                    case InterpolationMode.Constant:
		                interpolatedValue = v0Value;
		                break;
		            case InterpolationMode.Linear:
		                interpolatedValue = InterpolationUtility.Perspective(
		                    coordinates.Alpha, coordinates.Beta, coordinates.Gamma,
		                    ref v0Value, ref v1Value, ref v2Value,
		                    _p0.W, _p1.W, _p2.W, w);
		                break;
		            case InterpolationMode.LinearNoPerspective:
		                interpolatedValue = InterpolationUtility.Linear(
		                    coordinates.Alpha, coordinates.Beta, coordinates.Gamma,
		                    ref v0Value, ref v1Value, ref v2Value);
		                break;
                    case InterpolationMode.Undefined:
                        // TODO: This is used for SV_Position - can we just skip that semantic altogether?
                        interpolatedValue = new Number4();
                        break;
                    default:
		                throw new NotImplementedException();
		        }

		        // Set value onto pixel shader input.
		        result[i] = interpolatedValue;

		        // TODO: Do something different if input parameter is a system value.
		    }
		    return result;
		}
	}
}