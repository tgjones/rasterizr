using System;
using Nexus;
using Rasterizr.PipelineStages.OutputMerger;

namespace Rasterizr.Util
{
	public static class BlendUtility
	{
		public static ColorF DoColorBlend(BlendState blendState, ColorF source, ColorF destination)
		{
			ColorF left = source * GetBlendFactor(blendState.ColorSourceBlend, source, destination, blendState.BlendFactor);
			ColorF right = destination * GetBlendFactor(blendState.ColorDestinationBlend, source, destination, blendState.BlendFactor);

			return DoBlendOperation(blendState.ColorBlendFunction, left, right);
		}

		private static ColorF DoBlendOperation(BlendFunction blendFunction, ColorF left, ColorF right)
		{
			switch (blendFunction)
			{
				case BlendFunction.Add :
					return left + right;
				default :
					throw new NotSupportedException();
			}
		}

		private static ColorF GetBlendFactor(Blend blend, ColorF source, ColorF destination, ColorF blendFactor)
		{
			switch (blend)
			{
				case Blend.Zero:
					return ColorsF.Black;
				case Blend.One:
					return ColorsF.White;
				case Blend.SourceColor :
					return source;
				case Blend.InverseSourceColor :
					return ColorF.Invert(source);
				case Blend.SourceAlpha :
					return new ColorF(source.A, source.A, source.A, source.A);
				case Blend.InverseSourceAlpha :
					return ColorF.Invert(new ColorF(source.A, source.A, source.A, source.A));
				case Blend.DestinationAlpha :
					return new ColorF(destination.A, destination.A, destination.A, destination.A);
				case Blend.InverseDestinationAlpha:
					return ColorF.Invert(new ColorF(destination.A, destination.A, destination.A, destination.A));
				case Blend.DestinationColor :
					return destination;
				case Blend.InverseDestinationColor :
					return ColorF.Invert(destination);
				case Blend.SourceAlphaSaturation :
					return ColorF.Saturate(new ColorF(source.A, source.A, source.A, source.A));
				case Blend.BlendFactor :
					return blendFactor;
				case Blend.InverseBlendFactor :
					return ColorF.Invert(blendFactor);
				default :
					throw new NotSupportedException();
			}
		}
	}
}