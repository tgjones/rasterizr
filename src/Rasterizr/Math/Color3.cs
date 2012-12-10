using System.Runtime.InteropServices;

namespace Rasterizr.Math
{
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