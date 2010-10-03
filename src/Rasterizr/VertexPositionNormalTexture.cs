using System.Runtime.InteropServices;
using Nexus;
using Rasterizr.PipelineStages.InputAssembler;
using Rasterizr.VertexAttributes;

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
						new InputElementDescription("Position", VertexAttributeValueFormat.Point3D, InputElementUsage.Position),
						new InputElementDescription("Normal", VertexAttributeValueFormat.Vector3D, InputElementUsage.Normal),
						new InputElementDescription("TextureCoordinate", VertexAttributeValueFormat.Point2D, InputElementUsage.TextureCoordinate)
					}
				};
			}
		}
	}
}