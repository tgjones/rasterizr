using Rasterizr.Math;
using Rasterizr.Pipeline.Rasterizer;

namespace Rasterizr.Pipeline.PixelShader
{
	internal struct Pixel
	{
		public int X;
		public int Y;
		public Color4F Color;
		public Samples Samples;
		public int PrimitiveID;

		public Pixel(int x, int y)
		{
			X = x;
			Y = y;
			Color = new Color4F();
			Samples = new Samples();
			PrimitiveID = 0;
		}
	}
}