namespace Rasterizr.VertexAttributes
{
	public enum VertexAttributeInterpolationModifier
	{
		/// <summary>
		/// Interpolate between shader inputs; linear is the default value 
		/// if no interpolation modifier is specified.
		/// </summary>
		Linear,

		/// <summary>
		/// Interpolate between samples that are somewhere within the covered area of the pixel 
		/// (this may require extrapolating end points from a pixel center). Centroid sampling 
		/// may improve antialiasing if a pixel is partially covered 
		/// (even if the pixel center is not covered). 
		/// </summary>
		Centroid
	}
}