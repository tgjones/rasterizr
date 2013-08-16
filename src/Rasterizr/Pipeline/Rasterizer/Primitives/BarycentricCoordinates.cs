namespace Rasterizr.Pipeline.Rasterizer.Primitives
{
	internal struct BarycentricCoordinates
	{
		public float Alpha;
		public float Beta;
		public float Gamma;

		public bool IsOutsideTriangle
		{
			get { return (Alpha < 0 || Beta < 0 || Gamma < 0); }
		}
	}
}