using System.Runtime.InteropServices;
using Rasterizr.Math;
using Rasterizr.Pipeline.InputAssembler;

namespace Rasterizr.Toolkit
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexPositionTexture
	{
		public Vector3 Position;
		public Vector2 TextureCoordinate;

		public VertexPositionTexture(Vector3 position, Vector2 textureCoordinate)
		{
			Position = position;
			TextureCoordinate = textureCoordinate;
		}

		public static InputElement[] InputElements
		{
			get
			{
				return new[]
				{
					new InputElement("POSITION", 0, Format.R32G32B32_Float, 0),
					new InputElement("TEXCOORD", 0, Format.R32G32_Float, 0)
				};
			}
		}
	}
}