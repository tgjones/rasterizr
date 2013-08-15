namespace Rasterizr.Pipeline.Rasterizer
{
	public struct Viewport
	{
		public int TopLeftX;
        public int TopLeftY;
		public int Width;
        public int Height;
		public float MinDepth;
		public float MaxDepth;

		public Viewport(int x, int y, int width, int height, float minZ, float maxZ)
		{
			TopLeftX = x;
			TopLeftY = y;
			Width = width;
			Height = height;
			MinDepth = minZ;
			MaxDepth = maxZ;
		}
	}
}