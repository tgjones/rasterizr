namespace Rasterizr.Pipeline.OutputMerger
{
	public struct RenderTargetBlendDescription
	{
		public bool IsBlendEnabled;
		public BlendOption SourceBlend;
		public BlendOption DestinationBlend;
		public BlendOperation BlendOperation;
		public BlendOption SourceAlphaBlend;
		public BlendOption DestinationAlphaBlend;
		public BlendOperation AlphaBlendOperation;
		public ColorWriteMaskFlags RenderTargetWriteMask;
	}
}