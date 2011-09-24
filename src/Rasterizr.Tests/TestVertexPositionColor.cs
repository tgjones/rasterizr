using System.Runtime.InteropServices;
using Nexus;
using Rasterizr.Core.InputAssembler;
using Rasterizr.Core.ShaderCore;

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