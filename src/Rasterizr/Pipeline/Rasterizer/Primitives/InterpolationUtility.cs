using Rasterizr.Math;

namespace Rasterizr.Pipeline.Rasterizer.Primitives
{
	internal static class InterpolationUtility
	{
		public static Vector4 Linear(float alpha, float beta, float gamma,
			ref Vector4 p1, ref Vector4 p2, ref Vector4 p3)
		{
			return new Vector4
			{
				X = Linear(alpha, beta, gamma, p1.X, p2.X, p3.X),
				Y = Linear(alpha, beta, gamma, p1.Y, p2.Y, p3.Y),
				Z = Linear(alpha, beta, gamma, p1.Z, p2.Z, p3.Z),
				W = Linear(alpha, beta, gamma, p1.W, p2.W, p3.W)
			};
		}

		public static float Linear(float alpha, float beta, float gamma,
			float p1, float p2, float p3)
		{
			return p1 * alpha + p2 * beta + p3 * gamma;
		}

		public static float PrecalculateW(
			float alpha, float beta, float gamma,
			float p1W, float p2W, float p3W)
		{
			return (1f / p1W) * alpha + (1f / p2W) * beta + (1f / p3W) * gamma;
		}

		public static Vector4 Perspective(
			float alpha, float beta, float gamma,
			ref Vector4 p1, ref Vector4 p2, ref Vector4 p3,
			float p1W, float p2W, float p3W,
			float w)
		{
			var result = new Vector4
			{
				X = Perspective(alpha, beta, gamma, p1.X, p2.X, p3.X, p1W, p2W, p3W, w),
				Y = Perspective(alpha, beta, gamma, p1.Y, p2.Y, p3.Y, p1W, p2W, p3W, w),
				Z = Perspective(alpha, beta, gamma, p1.Z, p2.Z, p3.Z, p1W, p2W, p3W, w),
				W = Perspective(alpha, beta, gamma, p1.W, p2.W, p3.W, p1W, p2W, p3W, w)
			};
			return result;
		}

		public static float Perspective(
			float alpha, float beta, float gamma,
			float p1, float p2, float p3,
			float p1W, float p2W, float p3W,
			float w)
		{
			return ((p1 / p1W) * alpha
				+ (p2 / p2W) * beta
				+ (p3 / p3W) * gamma) / w;
		}
	}
}