using System.Runtime.InteropServices;

namespace Rasterizr.Math
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Color4F
	{
		public float Red;
		public float Green;
		public float Blue;
		public float Alpha;

		public Color4F(float red, float green, float blue, float alpha)
		{
			Red = red;
			Green = green;
			Blue = blue;
			Alpha = alpha;
		}
	}

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

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Color4
	{
		public byte Red;
		public byte Green;
		public byte Blue;
		public byte Alpha;

		public Color4(byte red, byte green, byte blue, byte alpha)
		{
			Red = red;
			Green = green;
			Blue = blue;
			Alpha = alpha;
		}
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Color3
	{
		public byte Red;
		public byte Green;
		public byte Blue;

		public Color3(byte red, byte green, byte blue)
		{
			Red = red;
			Green = green;
			Blue = blue;
		}
	}
}