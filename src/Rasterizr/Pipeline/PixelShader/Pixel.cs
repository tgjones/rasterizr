using Rasterizr.Pipeline.Rasterizer;
using Rasterizr.Pipeline.VertexShader;
using SlimShader;

namespace Rasterizr.Pipeline.PixelShader
{
	internal struct Pixel
	{
	    public VertexShaderOutput[] Vertices;
		public int X;
		public int Y;
        public Number4 Color;
		public Samples Samples;
		public int PrimitiveID;
	    public uint RenderTargetArrayIndex;

		public Pixel(int x, int y)
		{
		    Vertices = null;
			X = x;
			Y = y;
			Color = new Number4();
			Samples = new Samples();
			PrimitiveID = 0;
		    RenderTargetArrayIndex = 0;
		}
	}
}