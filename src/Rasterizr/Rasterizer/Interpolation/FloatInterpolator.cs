namespace Rasterizr.Rasterizer.Interpolation
{
	public class FloatInterpolator : ValueInterpolator<float>
	{
		public static float InterpolateLinear(float alpha, float beta, float gamma,
			float p1, float p2, float p3)
		{
			return p1 * alpha + p2 * beta + p3 * gamma;
		}

		public override float Linear(float alpha, float beta, float gamma,
			float p1, float p2, float p3)
		{
			return InterpolateLinear(alpha, beta, gamma, p1, p2, p3);
		}

		public override float Perspective(float alpha, float beta, float gamma,
			float p1, float p2, float p3,
			float p1W, float p2W, float p3W)
		{
			float w = (1f / p1W) * alpha + (1f / p2W) * beta + (1f / p3W) * gamma;

			float xx = (p1 / p1W) * alpha
				+ (p2 / p2W) * beta
					+ (p3 / p3W) * gamma;

			float correctedX = xx / w;
			return correctedX;
		}
	}
}