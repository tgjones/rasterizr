using System.Runtime.InteropServices;

namespace Rasterizr.Math
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Color4F
	{
		public float R;
		public float G;
		public float B;
		public float A;

		public Color4F(float r, float g, float b, float a)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}

		public static Color4F Invert(Color4F value)
		{
			return new Color4F(1 - value.A, 1 - value.R, 1 - value.G, 1 - value.B);
		}

		public static Color4F Invert(ref Color4F value)
		{
			return new Color4F(1 - value.A, 1 - value.R, 1 - value.G, 1 - value.B);
		}

		public static Color4F Multiply(ref Color4F left, ref Color4F right)
		{
			return new Color4F(
				left.R * right.R, 
				left.G * right.G, 
				left.B * right.B, 
				left.A * right.A);
		}

		public static Color4F Saturate(ref Color4F value)
		{
			return new Color4F(
				Saturate(value.R),
				Saturate(value.G),
				Saturate(value.B),
				1);
		}

		public static Color4F Saturate(Color4F value)
		{
			return new Color4F(
				Saturate(value.R),
				Saturate(value.G),
				Saturate(value.B),
				1);
		}

		private static float Saturate(float value)
		{
			value = (value > 1.0f) ? 1.0f : value;
			value = (value < 0.0f) ? 0.0f : value;
			return value;
		}

		public Color4 ToColor4()
		{
			return new Color4(
				(byte) (R * 255.0f),
				(byte) (G * 255.0f),
				(byte) (B * 255.0f),
				(byte) (A * 255.0f));
		}
	}
}