using System;
using Rasterizr.Math;

namespace Rasterizr.Pipeline.OutputMerger
{
	public class BlendState : DeviceChild
	{
		private readonly BlendStateDescription _description;

		public BlendStateDescription Description
		{
			get { return _description; }
		}

		internal BlendState(Device device, BlendStateDescription description)
			: base(device)
		{
			_description = description;
		}

		internal Color4F DoBlend(int renderTargetIndex, Color4F source, Color4F destination, Color4F blendFactor)
		{
			var blendDescription = Description.RenderTarget[renderTargetIndex];
			if (!blendDescription.IsBlendEnabled)
				return source;

			var result = new Color4F();

			// RGB blending
			var colorDestinationBlendFactor = GetBlendFactor(blendDescription.DestinationBlend, ref source, ref destination, ref blendFactor);
			var colorSourceBlendFactor = GetBlendFactor(blendDescription.SourceBlend, ref source, ref destination, ref blendFactor);

			var colorDestination = Color4F.Multiply(ref destination, ref colorDestinationBlendFactor);
			var colorSource = Color4F.Multiply(ref source, ref colorSourceBlendFactor);

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

		private Color4F GetBlendFactor(BlendOption blend, ref Color4F source, ref Color4F destination, ref Color4F blendFactor)
		{
			switch (blend)
			{
				case BlendOption.Zero:
					return new Color4F(0, 0, 0, 0);
				case BlendOption.One:
					return new Color4F(1, 1, 1, 1);
				case BlendOption.SourceColor:
					return source;
				case BlendOption.InverseSourceColor:
					return Color4F.Invert(ref source);
				case BlendOption.SourceAlpha:
					return new Color4F(source.A, source.A, source.A, source.A);
				case BlendOption.InverseSourceAlpha:
					return Color4F.Invert(new Color4F(source.A, source.A, source.A, source.A));
				case BlendOption.DestinationAlpha:
					return new Color4F(destination.A, destination.A, destination.A, destination.A);
				case BlendOption.InverseDestinationAlpha:
					return Color4F.Invert(new Color4F(destination.A, destination.A, destination.A, destination.A));
				case BlendOption.DestinationColor:
					return destination;
				case BlendOption.InverseDestinationColor:
					return Color4F.Invert(destination);
				case BlendOption.SourceAlphaSaturate:
					return Color4F.Saturate(new Color4F(source.A, source.A, source.A, source.A));
				case BlendOption.BlendFactor:
					return blendFactor;
				case BlendOption.InverseBlendFactor:
					return Color4F.Invert(blendFactor);
				default:
					throw new NotSupportedException();
			}
		}
	}
}