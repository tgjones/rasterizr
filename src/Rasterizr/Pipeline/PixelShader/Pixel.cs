using Rasterizr.Pipeline.Rasterizer;
using SlimShader;

namespace Rasterizr.Pipeline.PixelShader
{
	internal struct Pixel
	{
		public int X;
		public int Y;
        public Number4 Color;
		public Samples Samples;
		public int PrimitiveID;

		public Pixel(int x, int y)
		{
			X = x;
			Y = y;
			Color = new Number4();
			Samples = new Samples();
			PrimitiveID = 0;
		}
	}
}