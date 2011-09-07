using System;
using Nexus;

namespace Rasterizr.OutputMerger
{
	public class BlendState
	{
		#region Static stuff

		/// <summary>
		/// A built-in state object with settings for additive blend, that is adding the destination data to the source data without using alpha.
		/// </summary>
		public static readonly BlendState Additive;

		/// <summary>
		/// A built-in state object with settings for alpha blend, that is blending the source and destination data using alpha.
		/// </summary>
		public static readonly BlendState AlphaBlend;

		/// <summary>
		/// A built-in state object with settings for opaque blend, that is overwriting the source with the destination data.
		/// </summary>
		public static readonly BlendState Opaque;

		/// <summary>
		/// A built-in state object with settings for blending with non-premultipled alpha, that is blending source and destination data using alpha while assuming the color data contains no alpha information.
		/// </summary>
		public static readonly BlendState NonPremultiplied;

		static BlendState()
		{
			Opaque = new BlendState(Blend.One, Blend.Zero);
			AlphaBlend = new BlendState(Blend.One, Blend.InverseSourceAlpha);
			Additive = new BlendState(Blend.SourceAlpha, Blend.One);
			NonPremultiplied = new BlendState(Blend.SourceAlpha, Blend.InverseSourceAlpha);
		}

		#endregion

		public bool BlendEnable { get; set; }
		public Blend ColorSourceBlend { get; set; }
		public Blend ColorDestinationBlend { get; set; }
		public BlendFunction ColorBlendFunction { get; set; }
		public Blend AlphaSourceBlend { get; set; }
		public Blend AlphaDestinationBlend { get; set; }
		public BlendFunction AlphaBlendFunction { get; set; }
		public ColorF BlendFactor { get; set; }

		public BlendState(Blend sourceBlend, Blend destinationBlend)
			: this()
		{
			BlendEnable = true;
			ColorSourceBlend = AlphaSourceBlend = sourceBlend;
			ColorDestinationBlend = AlphaDestinationBlend = destinationBlend;
		}

		public BlendState()
		{
			BlendEnable = false;
			ColorSourceBlend = Blend.One;
			ColorDestinationBlend = Blend.Zero;
			ColorBlendFunction = BlendFunction.Add;
			BlendFactor = ColorsF.White;
		}

		public ColorF DoBlend(ColorF source, ColorF destination)
		{
			if (!BlendEnable)
				return source;

			var result = ColorsF.Empty;

			// RGB blending
			var colorDestinationBlendFactor = GetBlendFactor(ColorDestinationBlend, source, destination);
			var colorSourceBlendFactor = GetBlendFactor(ColorSourceBlend, source, destination);

			var colorDestination = destination * colorDestinationBlendFactor;
			var colorSource = source * colorSourceBlendFactor;

			result.R = DoBlendOperation(ColorBlendFunction, colorSource.R, colorDestination.R);
			result.G = DoBlendOperation(ColorBlendFunction, colorSource.G, colorDestination.G);
			result.B = DoBlendOperation(ColorBlendFunction, colorSource.B, colorDestination.B);

			// Alpha blending
			var alphaDestinationBlendFactor = GetBlendFactor(AlphaDestinationBlend, source, destination);
			var alphaSourceBlendFactor = GetBlendFactor(AlphaSourceBlend, source, destination);

			var alphaDestination = destination.A * alphaDestinationBlendFactor.A;
			var alphaSource = source.A * alphaSourceBlendFactor.A;

			result.A = DoBlendOperation(AlphaBlendFunction, alphaSource, alphaDestination);

			return result;
		}

		private static float DoBlendOperation(BlendFunction blendFunction, float left, float right)
		{
			switch (blendFunction)
			{
				case BlendFunction.Add:
					return left + right;
				default:
					throw new NotSupportedException();
			}
		}

		private ColorF GetBlendFactor(Blend blend, ColorF source, ColorF destination)
		{
			switch (blend)
			{
				case Blend.Zero:
					return new ColorF(0, 0, 0, 0);
				case Blend.One:
					return new ColorF(1, 1, 1, 1);
				case Blend.SourceColor:
					return source;
				case Blend.InverseSourceColor:
					return ColorF.Invert(source);
				case Blend.SourceAlpha:
					return new ColorF(source.A, source.A, source.A, source.A);
				case Blend.InverseSourceAlpha:
					return ColorF.Invert(new ColorF(source.A, source.A, source.A, source.A));
				case Blend.DestinationAlpha:
					return new ColorF(destination.A, destination.A, destination.A, destination.A);
				case Blend.InverseDestinationAlpha:
					return ColorF.Invert(new ColorF(destination.A, destination.A, destination.A, destination.A));
				case Blend.DestinationColor:
					return destination;
				case Blend.InverseDestinationColor:
					return ColorF.Invert(destination);
				case Blend.SourceAlphaSaturation:
					return ColorF.Saturate(new ColorF(source.A, source.A, source.A, source.A));
				case Blend.BlendFactor:
					return BlendFactor;
				case Blend.InverseBlendFactor:
					return ColorF.Invert(BlendFactor);
				default:
					throw new NotSupportedException();
			}
		}
	}
}