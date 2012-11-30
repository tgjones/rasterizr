namespace Rasterizr.Pipeline.OutputMerger
{
	public struct BlendStateDescription
	{
		public static BlendStateDescription Default
		{
			get
			{
				var result = new BlendStateDescription();
				result.RenderTarget[0] = new RenderTargetBlendDescription
				{
					IsBlendEnabled = false,
					SourceBlend = BlendOption.One,
					DestinationBlend = BlendOption.Zero,
					BlendOperation = BlendOperation.Add,
					SourceAlphaBlend = BlendOption.One,
					DestinationAlphaBlend = BlendOption.Zero,
					AlphaBlendOperation = BlendOperation.Add,
					RenderTargetWriteMask = ColorWriteMaskFlags.All
				};
				return result;
			}
		}

		/// <summary>
		/// A built-in state object with settings for additive blend, that is adding the destination data to the source data without using alpha.
		/// </summary>
		public static BlendStateDescription Additive
		{
			get { return new BlendStateDescription(BlendOption.SourceAlpha, BlendOption.One); }
		}

		/// <summary>
		/// A built-in state object with settings for alpha blend, that is blending the source and destination data using alpha.
		/// </summary>
		public static BlendStateDescription AlphaBlend
		{
			get { return new BlendStateDescription(BlendOption.One, BlendOption.InverseSourceAlpha); }
		}

		/// <summary>
		/// A built-in state object with settings for opaque blend, that is overwriting the source with the destination data.
		/// </summary>
		public static BlendStateDescription Opaque
		{
			get { return new BlendStateDescription(BlendOption.One, BlendOption.Zero); }
		}

		/// <summary>
		/// A built-in state object with settings for blending with non-premultipled alpha, that is blending source and destination data using alpha while assuming the color data contains no alpha information.
		/// </summary>
		public static BlendStateDescription NonPremultiplied
		{
			get { return new BlendStateDescription(BlendOption.SourceAlpha, BlendOption.InverseSourceAlpha); }
		}

		private RenderTargetBlendDescription[] _renderTarget;

		public bool AlphaToCoverageEnable;
		public bool IndependentBlendEnable;

		public RenderTargetBlendDescription[] RenderTarget
		{
			get { return _renderTarget ?? (_renderTarget = new RenderTargetBlendDescription[8]); }
		}

		public BlendStateDescription(BlendOption sourceBlend, BlendOption destinationBlend)
			: this()
		{
			RenderTarget[0] = new RenderTargetBlendDescription
			{
				IsBlendEnabled = true,
				SourceBlend = sourceBlend,
				SourceAlphaBlend = sourceBlend,
				DestinationBlend = destinationBlend,
				DestinationAlphaBlend = destinationBlend
			};
		}
	}
}