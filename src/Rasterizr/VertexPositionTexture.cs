using System.Runtime.InteropServices;
using Nexus;
using Rasterizr.PipelineStages.InputAssembler;
using Rasterizr.VertexAttributes;

namespace Rasterizr
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

		public static InputLayout InputLayout
		{
			get
			{
				return new InputLayout
				{
					Elements = new[]
					{
						new InputElementDescription("Position", VertexAttributeValueFormat.Point3D, InputElementUsage.Position),
						new InputElementDescription("TextureCoordinate", VertexAttributeValueFormat.Point2D, InputElementUsage.TextureCoordinate)
					}
				};
			}
		}
	}
}