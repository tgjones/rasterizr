namespace Rasterizr.Math
{
	internal static class MathUtility
	{
	    public static int FloorToInt(float f)
	    {
	        return (int) System.Math.Floor(f);
	    }

        public static int CeilingToInt(float f)
        {
            return (int) System.Math.Ceiling(f);
        }

		public static float Log2(float d)
		{
			return (float) System.Math.Log(d, 2.0f);
		}
	}
}