using System.Runtime.InteropServices;

namespace Rasterizr.Math
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct Point
	{
		public static int SizeInBytes
		{
			get { return sizeof(float) * 2; }
		}

		public float X;
		public float Y;

        public Point(float x, float y)
		{
			X = x;
			Y = y;
		}
	}
}