using System.Runtime.InteropServices;
using SlimShader;

namespace Rasterizr.Toolkit
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Color
	{
        public static readonly int SizeInBytes = sizeof(byte) * 4;

		public static Color Black
		{
			get { return new Color(0, 0, 0, 255); }
		}

		public static Color White
		{
			get { return new Color(255, 255, 255, 255); }
		}

		public byte R;
		public byte G;
		public byte B;
		public byte A;

		public Color(byte r, byte g, byte b, byte a)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}

		public Number4 ToNumber4()
		{
			return new Number4(
				R / 255.0f,
				G / 255.0f,
				B / 255.0f,
				A / 255.0f);
		}
	}
}