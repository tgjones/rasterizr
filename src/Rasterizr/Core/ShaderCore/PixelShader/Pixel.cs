using Nexus;
using Rasterizr.Core.Rasterizer;

namespace Rasterizr.Core.ShaderCore.PixelShader
{
	public class Pixel
	{
		public int X;
		public int Y;
		public ColorF Color;
		public float Depth;
		public SampleCollection Samples;

		public Pixel(int x, int y)
		{
			X = x;
			Y = y;
			Samples = new SampleCollection();
		}
	}
}