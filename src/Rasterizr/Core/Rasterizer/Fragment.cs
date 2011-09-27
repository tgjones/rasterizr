namespace Rasterizr.Core.Rasterizer
{
	/// <summary>
	/// A potential pixel.
	/// </summary>
	public class Fragment
	{
		public int X;
		public int Y;
		public SampleCollection Samples;
		public object PixelShaderInput;
		public float Depth { get; set; }
		public FragmentQuadLocation QuadLocation { get; private set; }

		public Fragment(int x, int y, FragmentQuadLocation quadLocation)
		{
			QuadLocation = quadLocation;
			X = x;
			Y = y;
			Samples = new SampleCollection();
		}
	}
}