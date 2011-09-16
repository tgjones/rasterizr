using System;
using System.Collections.Generic;
using Nexus;
using Rasterizr.ShaderCore.VertexShader;

namespace Rasterizr.Rasterizer
{
	public class CullerSubStage : RasterizerSubStageBase
	{
		public CullMode CullMode { get; set; }

		public void Process(List<IVertexShaderOutput> inputs, List<IVertexShaderOutput> outputs)
		{
			for (int i = 0; i < inputs.Count; i += 3)
			{
				IVertexShaderOutput v1 = inputs[i + 0];
				IVertexShaderOutput v2 = inputs[i + 1];
				IVertexShaderOutput v3 = inputs[i + 2];

				if (!ShouldCull(v1.Position, v2.Position, v3.Position))
				{
					outputs.Add(v1);
					outputs.Add(v2);
					outputs.Add(v3);
				}
			}
		}

		private bool ShouldCull(Point4D vv0, Point4D vv1, Point4D vv2)
		{
			Vector4D newVariable = vv1 - vv0;
			Vector3D l0 = Vector3D.Normalize(new Vector3D(newVariable.X, newVariable.Y, newVariable.Z));
			Vector4D newVariable2 = vv2 - vv0;
			Vector3D l1 = Vector3D.Normalize(new Vector3D(newVariable2.X, newVariable2.Y, newVariable2.Z));
			Vector3D vector = Vector3D.Cross(l0, l1);

			switch (CullMode)
			{
				case CullMode.None:
					return false;
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