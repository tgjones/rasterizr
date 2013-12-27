namespace Rasterizr.Pipeline.Rasterizer
{
	/// <summary>
	/// Stores 2x2 fragments. Used by the rasterizer to process quads of fragments, instead
	/// of individual fragments, so that the pixel shader can access partial derivatives.
	/// </summary>
	internal struct FragmentQuad
	{
		public Fragment Fragment0;
		public Fragment Fragment1;
		public Fragment Fragment2;
		public Fragment Fragment3;
	}
}