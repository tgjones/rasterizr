using System.Runtime.InteropServices;

namespace Rasterizr.Math
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Color4
	{
		public byte R;
		public byte G;
		public byte B;
		public byte A;

		public Color4(byte r, byte g, byte b, byte a)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}

		public Color4F ToColor4F()
		{
			return new Color4F(
				R / 255.0f,
				G / 255.0f,
				B / 255.0f,
				A / 255.0f);
		}
	}
}