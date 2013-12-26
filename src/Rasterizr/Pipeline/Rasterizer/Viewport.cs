using SlimShader;

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

        /// <summary>
        /// Converts from clip space to screen space.
        /// Formulae from http://msdn.microsoft.com/en-us/library/bb205126(v=vs.85).aspx
        /// </summary>
        public void MapClipSpaceToScreenSpace(ref Number4 position)
        {
            position.X = (position.X + 1) * Width * 0.5f + TopLeftX;
            position.Y = (1 - position.Y) * Height * 0.5f + TopLeftY;
            position.Z = MinDepth + position.Z * (MaxDepth - MinDepth);
        }
	}
}