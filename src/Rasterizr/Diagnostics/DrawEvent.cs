using Rasterizr.Math;
using Rasterizr.Pipeline.InputAssembler;

namespace Rasterizr.Diagnostics
{
	public class DrawEvent : PixelEvent
	{
		public PrimitiveTopology PrimitiveTopology { get; set; }
		public int PrimitiveID { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public PixelExclusionReason ExclusionReason { get; set; }
		public Color4F Previous { get; set; }
		public Color4F PixelShader { get; set; }
		public Color4F Result { get; set; }

		public override bool Matches(int x, int y)
		{
			return X == x && Y == y;
		}
	}
}