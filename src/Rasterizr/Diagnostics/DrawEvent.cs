﻿using Rasterizr.Pipeline.InputAssembler;
using SlimShader;

namespace Rasterizr.Diagnostics
{
	public class DrawEvent : PixelEvent
	{
		public PrimitiveTopology PrimitiveTopology { get; set; }
		public int PrimitiveID { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public PixelExclusionReason ExclusionReason { get; set; }
        public Number4 Previous { get; set; }
		public Number4 PixelShader { get; set; }
        public Number4 Result { get; set; }

		public override bool Matches(int x, int y)
		{
			return X == x && Y == y;
		}
	}
}