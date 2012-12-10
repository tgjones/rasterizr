using System.Runtime.InteropServices;

namespace Rasterizr.Math
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3
	{
		public static int SizeInBytes
		{
			get { return sizeof(float) * 3; }
		}

		public float X;
		public float Y;
		public float Z;

		public Vector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}
	}
}