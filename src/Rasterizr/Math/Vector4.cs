using System.Runtime.InteropServices;
using SlimShader;

namespace Rasterizr.Math
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Vector4
	{
		public float X;
		public float Y;
		public float Z;
		public float W;

		public Vector4(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public static Vector4 FromNumber4(Number4 value)
		{
			return new Vector4(value.Number0.Float, value.Number1.Float, value.Number2.Float, value.Number3.Float);
		}

		public Color4F ToColor4F()
		{
			return new Color4F(X, Y, Z, W);
		}

		public Number4 ToNumber4()
		{
			return new Number4(X, Y, Z, W);
		}
	}
}