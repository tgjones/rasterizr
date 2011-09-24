using System.Runtime.InteropServices;
using Nexus;
using Rasterizr.Core.InputAssembler;
using Rasterizr.Core.ShaderCore;

namespace Rasterizr
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexPositionColorTexture
	{
		public Point3D Position;
		public ColorF Color;
		public Point2D TextureCoordinate;

		public VertexPositionColorTexture(Point3D position, ColorF color, Point2D textureCoordinate)
		{
			Position = position;
			Color = color;
			TextureCoordinate = textureCoordinate;
		}

		public static InputLayout InputLayout
		{
			get
			{
				return new InputLayout
				{
					Elements = new[]
					{
						new InputElementDescription(Semantics.Position, 0),
						new InputElementDescription(Semantics.Color, 0),
						new InputElementDescription(Semantics.TexCoord, 0)
					}
				};
			}
		}
	}
}