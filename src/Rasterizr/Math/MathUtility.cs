namespace Rasterizr.Math
{
	internal static class MathUtility
	{
        internal static bool IsPowerOfTwo(int x)
        {
            return (x != 0) && (x & (x - 1)) == 0;
        }

		public static float Log2(float d)
		{
			return (float) System.Math.Log(d, 2.0f);
		}
	}
}