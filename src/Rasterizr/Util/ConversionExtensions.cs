using System;
using Rasterizr.Pipeline.InputAssembler;

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
	}
}