namespace Rasterizr.OutputMerger
{
	/// <summary>
	/// Defines color blending factors. 
	/// </summary>
	public enum Blend
	{
		/// <summary>
		/// Each component of the color is multiplied by (0, 0, 0, 0).
		/// </summary>
		Zero,

		/// <summary>
		/// Each component of the color is multiplied by (1, 1, 1, 1). 
		/// </summary>
		One,

		/// <summary>
		/// Each component of the color is multiplied by the source color.
		/// This can be represented as (Rs, Gs, Bs, As), where R, G, B, and A 
		/// respectively stand for the red, green, blue, and alpha source values.
		/// </summary>
		SourceColor,

		/// <summary>
		/// Each component of the color is multiplied by the inverse of the source color.
		/// This can be represented as (1 − Rs, 1 − Gs, 1 − Bs, 1 − As) where R, G, B, and A 
		/// respectively stand for the red, green, blue, and alpha destination values. 
		/// </summary>
		InverseSourceColor,

		/// <summary>
		/// Each component of the color is multiplied by the alpha value of the source. 
		/// This can be represented as (As, As, As, As), where As is the alpha source value. 
		/// </summary>
		SourceAlpha,

		/// <summary>
		/// Each component of the color is multiplied by the inverse of the alpha value 
		/// of the source. This can be represented as (1 − As, 1 − As, 1 − As, 1 − As), 
		/// where As is the alpha destination value. 
		/// </summary>
		InverseSourceAlpha,

		/// <summary>
		/// Each component of the color is multiplied by the alpha value of the destination. 
		/// This can be represented as (Ad, Ad, Ad, Ad), where Ad is the destination alpha value. 
		/// </summary>
		DestinationAlpha,

		/// <summary>
		/// Each component of the color is multiplied by the inverse of the alpha value 
		/// of the destination. This can be represented as (1 − Ad, 1 − Ad, 1 − Ad, 1 − Ad), 
		/// where Ad is the alpha destination value. 
		/// </summary>
		InverseDestinationAlpha,

		/// <summary>
		/// Each component color is multiplied by the destination color. This can be represented 
		/// as (Rd, Gd, Bd, Ad), where R, G, B, and A respectively stand for red, green, blue, 
		/// and alpha destination values. 
		/// </summary>
		DestinationColor,

		/// <summary>
		/// Each component of the color is multiplied by the inverse of the destination color. 
		/// This can be represented as (1 − Rd, 1 − Gd, 1 − Bd, 1 − Ad), where Rd, Gd, Bd, and 
		/// Ad respectively stand for the red, green, blue, and alpha destination values. 
		/// </summary>
		InverseDestinationColor,

		/// <summary>
		/// Each component of the color is multiplied by either the alpha of the source color, 
		/// or the inverse of the alpha of the source color, whichever is greater. This can be 
		/// represented as (f, f, f, 1), where f = min(A, 1 − Ad). 
		/// </summary>
		SourceAlphaSaturation,

		/// <summary>
		/// Each component of the color is multiplied by a constant set in BlendFactor. 
		/// </summary>
		BlendFactor,

		/// <summary>
		/// Each component of the color is multiplied by the inverse of a constant set in BlendFactor. 
		/// </summary>
		InverseBlendFactor
	}
}