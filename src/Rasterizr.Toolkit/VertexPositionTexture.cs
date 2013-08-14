using System.Runtime.InteropServices;
using Nexus;
using Rasterizr.Pipeline.InputAssembler;

namespace Rasterizr.Toolkit
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexPositionTexture
	{
		public Point3D Position;
		public Point2D TextureCoordinate;

		public VertexPositionTexture(Point3D position, Point2D textureCoordinate)
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