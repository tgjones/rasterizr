using Nexus.Graphics.Colors;
using Rasterizr.Rasterizer;

namespace Rasterizr.ShaderCore.PixelShader
{
	public class Pixel
	{
		public int X { get; private set; }
		public int Y { get; private set; }
		public ColorF Color { get; set; }
		public SampleCollection Samples { get; set; }

		public Pixel(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}