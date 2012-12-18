namespace Rasterizr.Diagnostics
{
	public enum PixelExclusionReason
	{
		NotExcluded,
		FailedDepthTest,
		FailedScissorTest,
		FailedStencilTest
	}
}