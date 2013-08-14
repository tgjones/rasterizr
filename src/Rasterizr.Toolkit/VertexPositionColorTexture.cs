using System.Runtime.InteropServices;
using Rasterizr.Pipeline.InputAssembler;
using SharpDX;

namespace Rasterizr.Toolkit
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexPositionColorTexture
	{
        public Vector3 Position;
		public Color4F Color;
        public Vector2 TextureCoordinate;

        public VertexPositionColorTexture(Vector3 position, Color4F color, Vector2 textureCoordinate)
		{
			Position = position;
			Color = color;
			TextureCoordinate = textureCoordinate;
		}

		public static InputElement[] InputElements
		{
			get
			{
				return new[]
				{
					new InputElement("POSITION", 0, Format.R32G32B32_Float, 0),
					new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 0),
					new InputElement("TEXCOORD", 0, Format.R32G32_Float, 0)
				};
			}
		}
	}
}