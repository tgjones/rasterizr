using System;

namespace Rasterizr.PipelineStages.Rasterizer
{
	public static class CullingUtility
	{
		public static bool ShouldCull(float determinant, CullMode cullMode)
		{
			switch (cullMode)
			{
				case CullMode.None :
					return false;
				case CullMode.CullClockwiseFace :
					return determinant > 0;
				case CullMode.CullCounterClockwiseFace :
					return determinant < 0;
				default :
					throw new NotSupportedException();
			}
		}
	}
}