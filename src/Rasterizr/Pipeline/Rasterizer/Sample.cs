namespace Rasterizr.Pipeline.Rasterizer
{
	public struct Sample
	{
		/// <summary>
		/// Gets or sets a flag which indicates whether this sample location 
		/// is covered by the polygon being rasterized.
		/// </summary>
		public bool Covered;

		public float Depth;
	}
}