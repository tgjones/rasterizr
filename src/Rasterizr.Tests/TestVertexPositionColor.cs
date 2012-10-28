using System.Runtime.InteropServices;
using Nexus;
using Nexus.Graphics.Colors;
using Rasterizr.InputAssembler;
using Rasterizr.ShaderCore;

namespace Rasterizr.Tests
{
	[StructLayout(LayoutKind.Sequential)]
	public struct TestVertexPositionColor
	{
		public Point3D Position;
		public ColorF Color;

		public TestVertexPositionColor(Point3D position, ColorF color)
		{
			Position = position;
			Color = color;
		}

		public static InputElementDescription[] InputElements
		{
			get
			{
				return new[]
				{
					new InputElementDescription(Semantics.Position, 0),
					new InputElementDescription(Semantics.Color, 0)
				};
			}
		}
	}
}