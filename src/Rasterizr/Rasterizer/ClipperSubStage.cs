using System.Collections.Generic;
using Nexus;
using Rasterizr.ShaderCore;

namespace Rasterizr.Rasterizer
{
	public class ClipperSubStage : RasterizerSubStageBase
	{
		public override IEnumerable<TransformedVertex> Process(IEnumerable<TransformedVertex> inputs)
		{
			var enumerator = inputs.GetEnumerator();
			while (enumerator.MoveNext())
			{
				TransformedVertex v1 = enumerator.Current;
				enumerator.MoveNext();
				TransformedVertex v2 = enumerator.Current;
				enumerator.MoveNext();
				TransformedVertex v3 = enumerator.Current;

				// TODO: This clipping isn't correct at all - it just throws away triangles if they're
				// partially or wholly outside. Need to improve it.
				if (!IsPartiallyOrWhollyOutsideViewport(v1.Position, v2.Position, v3.Position))
				{
					yield return v1;
					yield return v2;
					yield return v3;
				}
			}
		}

		private static bool IsPartiallyOrWhollyOutsideViewport(Point4D vv0, Point4D vv1, Point4D vv2)
		{
			if (vv0.X < -1 || vv0.X > 1 || vv0.Y < -1 || vv0.Y > 1 || vv0.Z < -1 || vv0.Z > 1)
				return true;

			if (vv1.X < -1 || vv1.X > 1 || vv1.Y < -1 || vv1.Y > 1 || vv1.Z < -1 || vv1.Z > 1)
				return true;

			if (vv2.X < -1 || vv2.X > 1 || vv2.Y < -1 || vv2.Y > 1 || vv2.Z < -1 || vv2.Z > 1)
				return true;

			return false;
		}
	}
}