using System.Diagnostics.Contracts;
using SlimShader;

namespace Rasterizr.Math
{
	internal struct Box2D
	{
		public int MinX;
		public int MinY;
		public int MaxX;
		public int MaxY;

		public Box2D(int minX, int minY, int maxX, int maxY)
		{
			MinX = minX;
			MinY = minY;
			MaxX = maxX;
			MaxY = maxY;
		}

        public static Box2D CreateBoundingBox(ref Number4 v0, ref Number4 v1, ref Number4 v2)
		{
			float minX = float.MaxValue, minY = float.MaxValue;
			float maxX = float.MinValue, maxY = float.MinValue;

			CheckMinMax(ref v0, ref minX, ref maxX, ref minY, ref maxY);
			CheckMinMax(ref v1, ref minX, ref maxX, ref minY, ref maxY);
			CheckMinMax(ref v2, ref minX, ref maxX, ref minY, ref maxY);

			return new Box2D(
                MathUtility.FloorToInt(minX), MathUtility.FloorToInt(minY),
                MathUtility.CeilingToInt(maxX), MathUtility.CeilingToInt(maxY));
		}

        private static void CheckMinMax(ref Number4 v,
			ref float minX, ref float maxX,
			ref float minY, ref float maxY)
		{
			if (v.X < minX)
				minX = v.X;
			if (v.X > maxX)
				maxX = v.X;

			if (v.Y < minY)
				minY = v.Y;
			if (v.Y > maxY)
				maxY = v.Y;
		}

        [Pure]
        public Box2D IntersectWith(ref Box2D other)
        {
            return new Box2D
            {
                MinX = System.Math.Max(MinX, other.MinX),
                MaxX = System.Math.Min(MaxX, other.MaxX),
                MinY = System.Math.Max(MinY, other.MinY),
                MaxY = System.Math.Min(MaxY, other.MaxY),
            };
        }

	    public bool IsPointInside(int x, int y)
	    {
	        return x >= MinX && x <= MaxX
	            && y >= MinY && y <= MaxY;
	    }
	}
}