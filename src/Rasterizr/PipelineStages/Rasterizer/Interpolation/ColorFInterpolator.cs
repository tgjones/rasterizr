using Nexus;

namespace Rasterizr.PipelineStages.Rasterizer.Interpolation
{
	public class ColorFInterpolator : ValueInterpolator<ColorF>
	{
		public override ColorF Linear(float alpha, float beta, float gamma,
			ColorF p1, ColorF p2, ColorF p3)
		{
			return p1 * alpha + p2 * beta + p3 * gamma;
		}

		public override ColorF Perspective(float alpha, float beta, float gamma,
			ColorF p1, ColorF p2, ColorF p3,
			float p1W, float p2W, float p3W)
		{
			float w = (1f / p1W) * alpha + (1f / p2W) * beta + (1f / p3W) * gamma;

			float rr = (p1.R / p1W) * alpha
				+ (p2.R / p2W) * beta
					+ (p3.R / p3W) * gamma;

			float gg = (p1.G / p1W) * alpha
				+ (p2.G / p2W) * beta
					+ (p3.G / p3W) * gamma;

			float bb = (p1.B / p1W) * alpha
				+ (p2.B / p2W) * beta
					+ (p3.B / p3W) * gamma;

			float aa = (p1.A / p1W) * alpha
				+ (p2.A / p2W) * beta
					+ (p3.A / p3W) * gamma;

			float correctedR = rr / w;
			float correctedG = gg / w;
			float correctedB = bb / w;
			float correctedA = aa / w;

			return new ColorF(correctedA, correctedR, correctedG, correctedB);
		}
	}
}