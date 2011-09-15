using System.Collections.Generic;
using Nexus;
using Rasterizr.ShaderStages.VertexShader;

namespace Rasterizr.Rasterizer
{
	public class ClipperSubStage : RasterizerSubStageBase
	{
		public void Process(List<IVertexShaderOutput> inputs, List<IVertexShaderOutput> outputs)
		{
			for (int i = 0; i < inputs.Count; i += 3)
			{
				IVertexShaderOutput v1 = inputs[i + 0];
				IVertexShaderOutput v2 = inputs[i + 1];
				IVertexShaderOutput v3 = inputs[i + 2];

				// TODO: This clipping isn't accurate at all - it just throws away triangles if they're
				// partially or wholly outside. Need to improve it.
				if (!IsPartiallyOrWhollyOutsideViewport(v1.Position, v2.Position, v3.Position))
				{
					outputs.Add(v1);
					outputs.Add(v2);
					outputs.Add(v3);
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