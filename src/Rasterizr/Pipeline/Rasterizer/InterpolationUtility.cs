using SlimShader;

namespace Rasterizr.Pipeline.Rasterizer
{
	internal static class InterpolationUtility
	{
		public static Number4 Linear(float alpha, float beta, float gamma,
			ref Number4 p1, ref Number4 p2, ref Number4 p3)
		{
			var result = new Number4
			{
				Number0 = { Float = Linear(alpha, beta, gamma, p1.Number0.Float, p2.Number0.Float, p3.Number0.Float) },
				Number1 = { Float = Linear(alpha, beta, gamma, p1.Number1.Float, p2.Number1.Float, p3.Number1.Float) },
				Number2 = { Float = Linear(alpha, beta, gamma, p1.Number2.Float, p2.Number2.Float, p3.Number2.Float) },
				Number3 = { Float = Linear(alpha, beta, gamma, p1.Number3.Float, p2.Number3.Float, p3.Number3.Float) }
			};
			return result;
		}

		public static float Linear(float alpha, float beta, float gamma,
			float p1, float p2, float p3)
		{
			return p1 * alpha + p2 * beta + p3 * gamma;
		}

		public static Number4 Perspective(float alpha, float beta, float gamma,
			ref Number4 p1, ref Number4 p2, ref Number4 p3,
			float p1W, float p2W, float p3W)
		{
			var result = new Number4
			{
				Number0 = { Float = Perspective(alpha, beta, gamma, p1.Number0.Float, p2.Number0.Float, p3.Number0.Float, p1W, p2W, p3W) },
				Number1 = { Float = Perspective(alpha, beta, gamma, p1.Number1.Float, p2.Number1.Float, p3.Number1.Float, p1W, p2W, p3W) },
				Number2 = { Float = Perspective(alpha, beta, gamma, p1.Number2.Float, p2.Number2.Float, p3.Number2.Float, p1W, p2W, p3W) },
				Number3 = { Float = Perspective(alpha, beta, gamma, p1.Number3.Float, p2.Number3.Float, p3.Number3.Float, p1W, p2W, p3W) }
			};
			return result;
		}

		public static float Perspective(float alpha, float beta, float gamma,
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