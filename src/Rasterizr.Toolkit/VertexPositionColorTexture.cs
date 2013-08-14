using System.Runtime.InteropServices;
using Nexus;
using Rasterizr.Pipeline.InputAssembler;

namespace Rasterizr.Toolkit
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexPositionColorTexture
	{
		public Point3D Position;
		public Color4F Color;
		public Point2D TextureCoordinate;

		public VertexPositionColorTexture(Point3D position, Color4F color, Point2D textureCoordinate)
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