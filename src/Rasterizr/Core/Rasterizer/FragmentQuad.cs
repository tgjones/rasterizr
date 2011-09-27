namespace Rasterizr.Core.Rasterizer
{
	/// <summary>
	/// Stores 2x2 fragments. Used by the rasterizer to process quads of fragments, instead
	/// of individual fragments, so that the pixel shader can access partial derivatives.
	/// </summary>
	public class FragmentQuad
	{
		public Fragment[] Fragments { get; private set; }

		public Fragment this[FragmentQuadLocation location]
		{
			get { return Fragments[(int) location]; }
			set { Fragments[(int) location] = value; }
		}

		public FragmentQuad()
		{
			Fragments = new Fragment[4];
		}
	}
}