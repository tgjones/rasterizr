namespace Rasterizr.Math
{
	public static class MathUtility
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

		public static int Floor(float value)
		{
			return (int) System.Math.Floor(value);
		}
	}
}