using Rasterizr.Math;

namespace Rasterizr.Pipeline.Rasterizer
{
	internal struct Fragment
	{
		public int PrimitiveID;
		public int X;
		public int Y;
		public FragmentQuadLocation QuadLocation;
		public Samples Samples;
		public Vector4[] Data;

		public Fragment(int primitiveID, int x, int y, FragmentQuadLocation quadLocation)
		{
			PrimitiveID = primitiveID;
			X = x;
			Y = y;
			QuadLocation = quadLocation;
			Samples = new Samples();
			Data = null;
		}
	}
}