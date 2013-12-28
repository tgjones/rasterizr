namespace Rasterizr.Math
{
	internal static class MathUtility
	{
        /// <summary>
        /// Clamps a value to an interval.
        /// </summary>
        /// <param name="value">The input parameter.</param>
        /// <param name="min">The lower clamp threshold.</param>
        /// <param name="max">The upper clamp threshold.</param>
        /// <returns>The clamped value.</returns>
        public static int Clamp(int value, int min, int max)
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;
            return value;
        }

        public static int CeilingToInt(float f)
        {
            return (int) System.Math.Ceiling(f);
        }

	    public static int FloorToInt(float f)
	    {
	        return (int) System.Math.Floor(f);
	    }

		public static float Log2(float d)
		{
			return (float) System.Math.Log(d, 2.0f);
		}
	}
}