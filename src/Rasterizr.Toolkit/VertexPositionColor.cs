using System.Runtime.InteropServices;
using Rasterizr.Pipeline.InputAssembler;
using SharpDX;

namespace Rasterizr.Toolkit
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexPositionColor
	{
		public Vector3 Position;
		public Color4F Color;

        public VertexPositionColor(Vector3 position, Color4F color)
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