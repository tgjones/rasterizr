using System.Runtime.InteropServices;
using Nexus;
using Rasterizr.PipelineStages.InputAssembler;
using Rasterizr.VertexAttributes;

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
						new InputElementDescription("Position", VertexAttributeValueFormat.Point3D),
						new InputElementDescription("Color", VertexAttributeValueFormat.ColorF),
						new InputElementDescription("TextureCoordinate", VertexAttributeValueFormat.Point2D)
					}
				};
			}
		}
	}
}