using System.Runtime.InteropServices;
using Nexus;
using Rasterizr.InputAssembler;
using Rasterizr.ShaderCore;

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

		public static InputElementDescription[] InputElements
		{
			get
			{
				return new[]
				{
					new InputElementDescription(Semantics.Position, 0),
					new InputElementDescription(Semantics.TexCoord, 0)
				};
			}
		}
	}
}