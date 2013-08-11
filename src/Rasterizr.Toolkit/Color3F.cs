using System.Runtime.InteropServices;

namespace Rasterizr.Toolkit
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Color3F
	{
		public static int SizeInBytes
		{
			get { return sizeof(float) * 3; }
		}

		public float Red;
		public float Green;
		public float Blue;

		public Color3F(float red, float green, float blue)
		{
			Red = red;
			Green = green;
			Blue = blue;
		}
	}
}