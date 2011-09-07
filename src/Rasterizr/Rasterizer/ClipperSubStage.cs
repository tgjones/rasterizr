using System.Collections.Concurrent;
using Nexus;
using Rasterizr.ShaderStages.VertexShader;

namespace Rasterizr.Rasterizer
{
	public class ClipperSubStage : RasterizerSubStageBase<IVertexShaderOutput, IVertexShaderOutput>
	{
		public override void Run(BlockingCollection<IVertexShaderOutput> inputs, BlockingCollection<IVertexShaderOutput> outputs)
		{
			try
			{
				var inputsEnumerator = inputs.GetConsumingEnumerable().GetEnumerator();
				while (inputsEnumerator.MoveNext())
				{
					IVertexShaderOutput v1 = inputsEnumerator.Current;
					inputsEnumerator.MoveNext();
					IVertexShaderOutput v2 = inputsEnumerator.Current;
					inputsEnumerator.MoveNext();
					IVertexShaderOutput v3 = inputsEnumerator.Current;

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
			finally
			{
				outputs.CompleteAdding();
			}
		}

		private bool IsPartiallyOrWhollyOutsideViewport(Point4D vv0, Point4D vv1, Point4D vv2)
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