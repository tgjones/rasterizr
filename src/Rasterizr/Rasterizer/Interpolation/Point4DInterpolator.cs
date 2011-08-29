using Nexus;

namespace Rasterizr.Rasterizer.Interpolation
{
	public class Point4DInterpolator : ValueInterpolator<Point4D>
	{
		public override Point4D Linear(float alpha, float beta, float gamma, 
			Point4D p1, Point4D p2, Point4D p3)
		{
			return p1 * alpha + p2 * beta + p3 * gamma;
		}

		public override Point4D Perspective(float alpha, float beta, float gamma,
			Point4D p1, Point4D p2, Point4D p3,
			float p1W, float p2W, float p3W)
		{
			float w = (1f / p1W) * alpha + (1f / p2W) * beta + (1f / p3W) * gamma;

			float xx = (p1.X / p1W) * alpha
				+ (p2.X / p2W) * beta
					+ (p3.X / p3W) * gamma;

			float yy = (p1.Y / p1W) * alpha
				+ (p2.Y / p2W) * beta
					+ (p3.Y / p3W) * gamma;

			float zz = (p1.Z / p1W) * alpha
				+ (p2.Z / p2W) * beta
					+ (p3.Z / p3W) * gamma;

			float ww = (p1.W / p1W) * alpha
				+ (p2.W / p2W) * beta
					+ (p3.W / p3W) * gamma;

			float correctedX = xx / w;
			float correctedY = yy / w;
			float correctedZ = zz / w;
			float correctedW = ww / w;
			
			return new Point4D(correctedX, correctedY, correctedZ, correctedW);
		}
	}
}