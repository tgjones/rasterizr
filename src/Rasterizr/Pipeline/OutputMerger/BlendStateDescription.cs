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

		private RenderTargetBlendDescription[] _renderTarget;

		public bool AlphaToCoverageEnable;
		public bool IndependentBlendEnable;

		public RenderTargetBlendDescription[] RenderTarget
		{
			get { return _renderTarget ?? (_renderTarget = new RenderTargetBlendDescription[8]); }
		}
	}
}