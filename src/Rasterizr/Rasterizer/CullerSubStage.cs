using System;
using System.Collections.Concurrent;
using Nexus;
using Rasterizr.ShaderStages.VertexShader;

namespace Rasterizr.Rasterizer
{
	public class CullerSubStage : RasterizerSubStageBase<IVertexShaderOutput, IVertexShaderOutput>
	{
		public CullMode CullMode { get; set; }

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

					if (!ShouldCull(v1.Position, v2.Position, v3.Position))
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