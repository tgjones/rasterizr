using System.Runtime.InteropServices;
using Nexus;
using Rasterizr.Core.InputAssembler;
using Rasterizr.Core.ShaderCore;

namespace Rasterizr
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexPositionNormalTexture
	{
		public Point3D Position;
		public Vector3D Normal;
		public Point2D TextureCoordinate;

		public VertexPositionNormalTexture(Point3D position, Vector3D normal, Point2D textureCoordinate)
		{
			Position = position;
			Normal = normal;
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
						new InputElementDescription(Semantics.Normal, 0),
						new InputElementDescription(Semantics.TexCoord, 0)
					}
				};
			}
		}
	}
}