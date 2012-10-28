using Nexus;

namespace Rasterizr.Rasterizer.Derivatives
{
	public class Point2DDerivativeValues : DerivativeValues<Point2D, Vector2D>
	{
		protected override Vector2D Subtract(Point2D v1, Point2D v2)
		{
			return v1 - v2;
		}
	}
}