using System;
using System.Collections.Generic;
using System.Diagnostics;
using Rasterizr.Math;
using Rasterizr.Pipeline.InputAssembler;
using SlimShader;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Pipeline.Rasterizer.Primitives
{
	internal class TriangleRasterizer : PrimitiveRasterizer
	{
	    private InputAssemblerPrimitiveOutput _primitive;
		private Number4 _p0, _p1, _p2;

		private float _alphaDenominator;
		private float _betaDenominator;
		private float _gammaDenominator;

	    public TriangleRasterizer(
            RasterizerStateDescription rasterizerState, 
            int multiSampleCount, 
            ShaderOutputInputBindings outputInputBindings, 
            ref Viewport viewport,
            Func<int, int, bool> fragmentFilter) 
            : base(rasterizerState, multiSampleCount, outputInputBindings, ref viewport, fragmentFilter)
	    {
	        
	    }

	    public override bool ShouldCull(VertexShader.VertexShaderOutput[] vertices)
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

		public override IEnumerable<FragmentQuad> Rasterize(InputAssemblerPrimitiveOutput primitive)
		{
		    _primitive = primitive;
            _p0 = _primitive.Vertices[0].Position;
            _p1 = _primitive.Vertices[1].Position;
            _p2 = _primitive.Vertices[2].Position;

			// Precompute alpha, beta and gamma denominator values. These are the same for all fragments.
			_alphaDenominator = ComputeFunction(_p0.X, _p0.Y, ref _p1, ref _p2);
			_betaDenominator = ComputeFunction(_p1.X, _p1.Y, ref _p2, ref _p0);
			_gammaDenominator = ComputeFunction(_p2.X, _p2.Y, ref _p0, ref _p1);

            // TODO: Handle degenerate triangles. The following code might be right, but need to check.
		    //const float degenerateThreshold = 0.001f;
            //if (_alphaDenominator < degenerateThreshold || _betaDenominator < degenerateThreshold || _gammaDenominator < degenerateThreshold)
            //    yield break;

            // Compute screen-space triangle bounding box.
			var screenBounds = Box2D.CreateBoundingBox(ref _p0, ref _p1, ref _p2);

            // Clip triangle bounding box to screen bounds.
		    screenBounds = ScreenBounds.IntersectWith(ref screenBounds);

			// Calculate start and end positions. Because of the need to calculate derivatives
			// in the pixel shader, we require that fragment quads always have even numbered
			// coordinates (both x and y) for the top-left fragment.
            var startX = RoundDownToEven(screenBounds.MinX);
            var startY = RoundDownToEven(screenBounds.MinY);

            // Scan pixels in target area, checking if they are inside the triangle.
            // If they are, calculate the coverage.
		    var maxX = screenBounds.MaxX;
		    var maxY = screenBounds.MaxY;
			for (int y = startY; y < maxY; y += 2)
				for (int x = startX; x < maxX; x += 2)
				{
				    if (FragmentFilter != null)
				    {
                        if (!FragmentFilter(x, y)
                            && !FragmentFilter(x + 1, y)
                            && !FragmentFilter(x, y + 1)
                            && !FragmentFilter(x + 1, y + 1))
                            continue;
				    }

				    // First check whether any fragments in this quad are covered. If not, we don't
					// need to do any (expensive) interpolation of attributes.
					var fragmentQuad = new FragmentQuad
					{
						Fragment0 = CreateFragment(x, y, FragmentQuadLocation.TopLeft, ref screenBounds),
                        Fragment1 = CreateFragment(x + 1, y, FragmentQuadLocation.TopRight, ref screenBounds),
                        Fragment2 = CreateFragment(x, y + 1, FragmentQuadLocation.BottomLeft, ref screenBounds),
                        Fragment3 = CreateFragment(x + 1, y + 1, FragmentQuadLocation.BottomRight, ref screenBounds),
					};

					if (RasterizerState.IsMultisampleEnabled)
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

	    private Fragment CreateFragment(int x, int y, FragmentQuadLocation quadLocation, ref Box2D screenBounds)
	    {
	        return new Fragment(
                _primitive.Vertices, 
                _primitive.PrimitiveID, 
                _primitive.RenderTargetArrayIndex,
                x, y, quadLocation,
	            screenBounds.IsPointInside(x, y));
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

		private static int RoundDownToEven(int value)
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

			// If any of these tests fails, the current pixel is not inside the triangle.
			if (coordinates.IsOutsideTriangle)
			{
				depth = 0;
				return false;
			}

            // Calculate depth.
            depth = InterpolationUtility.Linear(coordinates.Alpha, coordinates.Beta, coordinates.Gamma, _p0.Z, _p1.Z, _p2.Z);

            //// Clip to near and far planes.
            //if (depth < 0 || depth > 1)
            //    return false;

            //// Clip to 0 < w.
            //// TODO: Is this right?
            //// TODO: We do the same thing later on for attribute interpolations, can it be optimised?
            //var w = InterpolationUtility.Linear(coordinates.Alpha, coordinates.Beta, coordinates.Gamma, _p0.W, _p1.W, _p2.W);
            //if (w <= 0)
            //    return false;

			// The exact value to compare against depends on fill mode - if we're rendering wireframe,
			// then check whether sample position is within the "wireframe threshold" (i.e. 1 pixel) of an edge.
			switch (RasterizerState.FillMode)
			{
				case FillMode.Solid:
					return true;
				case FillMode.Wireframe:
                    // TODO: This threshold thing is not correct.
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

		    var v0Data = _primitive.Vertices[0].OutputData;
            var v1Data = _primitive.Vertices[1].OutputData;
            var v2Data = _primitive.Vertices[2].OutputData;

			// Calculate interpolated attribute values for this fragment.
            // TODO: Optimize this.
            var result = new Number4[_primitive.Vertices[0].OutputData.Length];
		    foreach (var outputInputBinding in OutputInputBindings.Bindings)
		    {
                var v0Value = v0Data[outputInputBinding.Register];
                var v1Value = v1Data[outputInputBinding.Register];
                var v2Value = v2Data[outputInputBinding.Register];

                if (outputInputBinding.SystemValueType != Name.Undefined)
                    throw new NotImplementedException();

                // Create input values. Normally, this will require interpolation
                // of the vertex attributes.
                Number4 inputValue;
                switch (outputInputBinding.InterpolationMode)
                {
                    case InterpolationMode.Constant:
                        inputValue = v0Value;
                        break;
                    case InterpolationMode.Linear:
                        inputValue = InterpolationUtility.Perspective(
                            coordinates.Alpha, coordinates.Beta, coordinates.Gamma,
                            ref v0Value, ref v1Value, ref v2Value,
                            _p0.W, _p1.W, _p2.W, w);
                        break;
                    case InterpolationMode.LinearNoPerspective:
                        inputValue = InterpolationUtility.Linear(
                            coordinates.Alpha, coordinates.Beta, coordinates.Gamma,
                            ref v0Value, ref v1Value, ref v2Value);
                        break;
                    default:
                        throw new InvalidOperationException("Unrecognised interpolation mode: " + outputInputBinding.InterpolationMode);
                }

                // Apply component mask so that we don't overwrite other values in this register.
		        result[outputInputBinding.Register].WriteMaskedValue(
                    inputValue, outputInputBinding.ComponentMask);
		    }
            
		    return result;
		}
	}
}