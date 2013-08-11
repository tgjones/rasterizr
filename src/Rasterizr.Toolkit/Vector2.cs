using System.Runtime.InteropServices;

namespace Rasterizr.Toolkit
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2
	{
		public static int SizeInBytes
		{
			get { return sizeof(float) * 2; }
		}

		public float X;
		public float Y;

		public Vector2(float x, float y)
		{
			X = x;
			Y = y;
		}
	}
}