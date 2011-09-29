namespace Rasterizr.Core.Rasterizer
{
	/// <summary>
	/// A potential pixel.
	/// </summary>
	public class Fragment
	{
		public int X { get; private set; }
		public int Y { get; private set; }
		public SampleCollection Samples { get; private set; }
		public object PixelShaderInput { get; set; }
		public FragmentQuadLocation QuadLocation { get; private set; }

		public Fragment(int x, int y, FragmentQuadLocation quadLocation,
			SampleCollection samples)
		{
			X = x;
			Y = y;
			QuadLocation = quadLocation;
			Samples = samples;
		}
	}
}