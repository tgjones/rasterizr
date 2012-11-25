namespace Rasterizr
{
	public struct SwapChainDescription
	{
		public int Width;
		public int Height;
		public Format Format;
		public SampleDescription SampleDescription;
	}

	public struct SampleDescription
	{
		public int Count;

		public SampleDescription(int count)
		{
			Count = count;
		}
	}
}