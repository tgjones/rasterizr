namespace Rasterizr.Rasterizer
{
	/// <summary>
	/// Stores 2x2 pixels. Used by the rasterizer to process quads of fragments, instead
	/// of individual fragments, so that the pixel shader can access partial derivatives.
	/// </summary>
	public class QuadFragment
	{
		public Fragment F1 { get; set; }
		public Fragment F2 { get; set; }
		public Fragment F3 { get; set; }
		public Fragment F4 { get; set; }

		public float CalculatePartialDerivative()
		{
			throw new System.NotImplementedException();
		}
	}

	/// <summary>
	/// Stores metadata for a particular pixel input type, such as the names and types
	/// of its properties, what type of interpolation is required, etc.
	/// </summary>
	public class FragmentType
	{
		
	}
}