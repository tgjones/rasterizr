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

		public static Box2D CreateBoundingBox(ref Vector4 v0, ref Vector4 v1, ref Vector4 v2)
		{
			float minX = float.MaxValue, minY = float.MaxValue;
			float maxX = float.MinValue, maxY = float.MinValue;

			CheckMinMax(ref v0, ref minX, ref maxX, ref minY, ref maxY);
			CheckMinMax(ref v1, ref minX, ref maxX, ref minY, ref maxY);
			CheckMinMax(ref v2, ref minX, ref maxX, ref minY, ref maxY);

			return new Box2D((int)minX, (int)minY, (int)maxX, (int)maxY);
		}

		private static void CheckMinMax(ref Vector4 v,
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
	}
}