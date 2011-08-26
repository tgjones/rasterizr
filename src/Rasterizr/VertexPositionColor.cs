using System.Runtime.InteropServices;
using Nexus;
using Rasterizr.PipelineStages.InputAssembler;
using Rasterizr.PipelineStages.ShaderStages.Core;

namespace Rasterizr
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexPositionColor
	{
		public Point3D Position;
		public ColorF Color;

		public VertexPositionColor(Point3D position, ColorF color)
		{
			Position = position;
			Color = color;
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
						new InputElementDescription(Semantics.Color, 0)
					}
				};
			}
		}
	}
}