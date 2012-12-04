using System;

namespace Rasterizr.Pipeline.OutputMerger
{
	[Flags]
	public enum DepthStencilViewFlags
	{
		None = 0,
		ReadOnlyDepth = 1,
		ReadOnlyStencil = 2
	}
}