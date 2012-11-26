using System;

namespace Rasterizr.Pipeline.OutputMerger
{
	[Flags]
	public enum ColorWriteMaskFlags : byte
	{
		Red = 1,
		Green = 2,
		Blue = 4,
		Alpha = 8,

		All = Red | Green | Blue | Alpha
	}
}