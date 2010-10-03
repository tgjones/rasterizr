using Nexus;

namespace Rasterizr.PipelineStages.ShaderStages.PixelShader
{
	public class Pixel
	{
		public int X;
		public int Y;
		public ColorF Color;
		public float Depth;

		public Pixel(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}