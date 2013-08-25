namespace Rasterizr
{
	public struct SwapChainDescription
	{
		public int Width;
		public int Height;
		public SampleDescription SampleDescription;

	    public SwapChainDescription(int width, int height)
	    {
	        Width = width;
	        Height = height;
            SampleDescription = new SampleDescription(1);
	    }
	}
}