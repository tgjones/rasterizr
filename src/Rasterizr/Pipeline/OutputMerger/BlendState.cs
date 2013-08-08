using System;
using SlimShader;

namespace Rasterizr.Pipeline.OutputMerger
{
	public class BlendState : DeviceChild
	{
		private readonly BlendStateDescription _description;
	    private readonly RenderTargetBlendDescription[] _renderTargetBlendDescriptions;

		public BlendStateDescription Description
		{
			get { return _description; }
		}

		internal BlendState(Device device, BlendStateDescription description)
			: base(device)
		{
			_description = description;
		    _renderTargetBlendDescriptions = description.RenderTarget;
		}

        internal Number4 DoBlend(int renderTargetIndex, ref Number4 source, ref Number4 destination, ref Number4 blendFactor)
		{
            var blendDescription = _renderTargetBlendDescriptions[renderTargetIndex];
			if (!blendDescription.IsBlendEnabled)
				return source;

            var result = new Number4();

			// RGB blending
			var colorDestinationBlendFactor = GetBlendFactor(blendDescription.DestinationBlend, ref source, ref destination, ref blendFactor);
			var colorSourceBlendFactor = GetBlendFactor(blendDescription.SourceBlend, ref source, ref destination, ref blendFactor);

			var colorDestination = Number4.Multiply(ref destination, ref colorDestinationBlendFactor);
            var colorSource = Number4.Multiply(ref source, ref colorSourceBlendFactor);

			result.R = DoBlendOperation(blendDescription.BlendOperation, colorSource.R, colorDestination.R);
			result.G = DoBlendOperation(blendDescription.BlendOperation, colorSource.G, colorDestination.G);
			result.B = DoBlendOperation(blendDescription.BlendOperation, colorSource.B, colorDestination.B);

			// Alpha blending
			var alphaDestinationBlendFactor = GetBlendFactor(blendDescription.DestinationAlphaBlend, ref source, ref destination, ref blendFactor);
			var alphaSourceBlendFactor = GetBlendFactor(blendDescription.SourceAlphaBlend, ref source, ref destination, ref blendFactor);

			var alphaDestination = destination.A * alphaDestinationBlendFactor.A;
			var alphaSource = source.A * alphaSourceBlendFactor.A;

			result.A = DoBlendOperation(blendDescription.AlphaBlendOperation, alphaSource, alphaDestination);

			return result;
		}

		private static float DoBlendOperation(BlendOperation blendFunction, float left, float right)
		{
			switch (blendFunction)
			{
				case BlendOperation.Add:
					return left + right;
				default:
					throw new NotSupportedException();
			}
		}

        private Number4 GetBlendFactor(BlendOption blend, ref Number4 source, ref Number4 destination, ref Number4 blendFactor)
		{
			switch (blend)
			{
				case BlendOption.Zero:
                    return new Number4(0, 0, 0, 0);
				case BlendOption.One:
                    return new Number4(1, 1, 1, 1);
				case BlendOption.SourceColor:
					return source;
				case BlendOption.InverseSourceColor:
                    return Number4.Invert(ref source);
				case BlendOption.SourceAlpha:
                    return new Number4(source.A, source.A, source.A, source.A);
				case BlendOption.InverseSourceAlpha:
                    return Number4.Invert(new Number4(source.A, source.A, source.A, source.A));
				case BlendOption.DestinationAlpha:
                    return new Number4(destination.A, destination.A, destination.A, destination.A);
				case BlendOption.InverseDestinationAlpha:
                    return Number4.Invert(new Number4(destination.A, destination.A, destination.A, destination.A));
				case BlendOption.DestinationColor:
					return destination;
				case BlendOption.InverseDestinationColor:
                    return Number4.Invert(ref destination);
				case BlendOption.SourceAlphaSaturate:
                    return Number4.Saturate(new Number4(source.A, source.A, source.A, source.A));
				case BlendOption.BlendFactor:
					return blendFactor;
				case BlendOption.InverseBlendFactor:
					return Number4.Invert(ref blendFactor);
				default:
					throw new NotSupportedException();
			}
		}
	}
}