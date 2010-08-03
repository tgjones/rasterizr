using System;

namespace Rasterizr.PipelineStages.Clipping
{
	[Flags]
	public enum ClipRegions
	{
		NotClipped = 0,
		Left = 1,
		Right = 2,
		Top = 4,
		Bottom = 8,
		Near = 16,
		Far = 32
	}
}