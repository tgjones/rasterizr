using Nexus;

namespace Rasterizr.Rasterizer.Interpolation
{
	public class Point2DInterpolator : ValueInterpolator<Point2D>
	{
		public override Point2D Linear(float alpha, float beta, float gamma,
			Point2D p1, Point2D p2, Point2D p3)
		{
			return p1 * alpha + p2 * beta + p3 * gamma;
		}

		public override Point2D Perspective(float alpha, float beta, float gamma,
			Point2D p1, Point2D p2, Point2D p3,
			float p1W, float p2W, float p3W)
		{
			float w = (1f / p1W) * alpha + (1f / p2W) * beta + (1f / p3W) * gamma;

			float xx = (p1.X / p1W) * alpha
				+ (p2.X / p2W) * beta
					+ (p3.X / p3W) * gamma;

			float yy = (p1.Y / p1W) * alpha
				+ (p2.Y / p2W) * beta
					+ (p3.Y / p3W) * gamma;

			float correctedX = xx / w;
			float correctedY = yy / w;

			return new Point2D(correctedX, correctedY);
		}
	}
}