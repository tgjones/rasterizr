using System;

namespace Rasterizr.Pipeline.OutputMerger
{
	[Flags]
	public enum DepthStencilClearFlags
	{
		Depth = 1,
		Stencil = 2
	}
}