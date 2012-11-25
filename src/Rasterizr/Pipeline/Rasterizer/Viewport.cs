namespace Rasterizr.Pipeline.Rasterizer
{
	public struct Viewport
	{
		public float TopLeftX;
		public float TopLeftY;
		public float Width;
		public float Height;
		public float MinDepth;
		public float MaxDepth;

		public Viewport(float x, float y, float width, float height, float minZ, float maxZ)
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