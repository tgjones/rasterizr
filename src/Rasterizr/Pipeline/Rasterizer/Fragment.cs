using Rasterizr.Pipeline.VertexShader;
using SlimShader;

namespace Rasterizr.Pipeline.Rasterizer
{
	internal struct Fragment
	{
	    public VertexShaderOutput[] Vertices;
		public int PrimitiveID;
		public int X;
		public int Y;
		public FragmentQuadLocation QuadLocation;
	    public bool IsInsideViewport;
		public Samples Samples;
		public Number4[] Data;

		public Fragment(VertexShaderOutput[] vertices, 
            int primitiveID, int x, int y, 
            FragmentQuadLocation quadLocation,
            bool isInsideViewport)
		{
		    Vertices = vertices;
			PrimitiveID = primitiveID;
			X = x;
			Y = y;
			QuadLocation = quadLocation;
		    IsInsideViewport = isInsideViewport;
			Samples = new Samples();
			Data = null;
		}
	}
}