using System;
using Rasterizr.Pipeline.InputAssembler;
using SlimShader;

namespace Rasterizr.Util
{
	internal static class ConversionExtensions
	{
		public static PrimitiveTopology ToPrimitiveTopology(this SlimShader.Chunks.Common.PrimitiveTopology primitiveTopology)
		{
			switch (primitiveTopology)
			{
				case SlimShader.Chunks.Common.PrimitiveTopology.PointList:
					return PrimitiveTopology.PointList;
				case SlimShader.Chunks.Common.PrimitiveTopology.LineStrip:
					return PrimitiveTopology.LineStrip;
				case SlimShader.Chunks.Common.PrimitiveTopology.TriangleStrip:
					return PrimitiveTopology.TriangleStrip;
				default:
					throw new ArgumentOutOfRangeException("primitiveTopology");
			}
		}

        public static Number4 ToNumber4(this Color4 color)
        {
            return new Number4(color.R, color.G, color.B, color.A);
        }
	}
}