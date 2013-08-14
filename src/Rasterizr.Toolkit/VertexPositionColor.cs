using System.Runtime.InteropServices;
using Nexus;
using Rasterizr.Pipeline.InputAssembler;

namespace Rasterizr.Toolkit
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexPositionColor
	{
		public Point3D Position;
		public Color4F Color;

        public VertexPositionColor(Point3D position, Color4F color)
		{
			Position = position;
			Color = color;
		}

		public static InputElement[] InputElements
		{
			get
			{
				return new[]
				{
					new InputElement("POSITION", 0, Format.R32G32B32_Float, 0),
					new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 0)
				};
			}
		}
	}
}