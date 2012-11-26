namespace Rasterizr.Pipeline.OutputMerger
{
	public struct BlendStateDescription
	{
		private RenderTargetBlendDescription[] _renderTarget;

		public bool AlphaToCoverageEnable;
		public bool IndependentBlendEnable;

		public RenderTargetBlendDescription[] RenderTarget
		{
			get { return _renderTarget ?? (_renderTarget = new RenderTargetBlendDescription[8]); }
		}
	}
}