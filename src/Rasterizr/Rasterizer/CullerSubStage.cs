using System;
using System.Collections.Generic;
using Nexus;
using Rasterizr.ShaderCore;

namespace Rasterizr.Rasterizer
{
	public class CullerSubStage : RasterizerSubStageBase
	{
		public CullMode CullMode { get; set; }

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

				if (!ShouldCull(v1.Position, v2.Position, v3.Position))
				{
					yield return v1;
					yield return v2;
					yield return v3;
				}
			}
		}

		private bool ShouldCull(Point4D vv0, Point4D vv1, Point4D vv2)
		{
			if (CullMode == CullMode.None)
				return false;

			Vector4D newVariable = vv1 - vv0;
			Vector3D l0 = Vector3D.Normalize(new Vector3D(newVariable.X, newVariable.Y, newVariable.Z));
			Vector4D newVariable2 = vv2 - vv0;
			Vector3D l1 = Vector3D.Normalize(new Vector3D(newVariable2.X, newVariable2.Y, newVariable2.Z));
			Vector3D vector = Vector3D.Cross(l0, l1);

			switch (CullMode)
			{
				case CullMode.CullClockwiseFace:
					return vector.Z > 0;
				case CullMode.CullCounterClockwiseFace:
					return vector.Z < 0;
				default:
					throw new NotSupportedException();
			}
		}
	}
}