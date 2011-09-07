using System.Collections.Concurrent;
using Rasterizr.ShaderStages.VertexShader;

namespace Rasterizr.Rasterizer
{
	public class PerspectiveDividerSubStage : RasterizerSubStageBase<IVertexShaderOutput, IVertexShaderOutput>
	{
		public override void Run(BlockingCollection<IVertexShaderOutput> inputs, BlockingCollection<IVertexShaderOutput> outputs)
		{
			try
			{
				foreach (var input in inputs.GetConsumingEnumerable())
				{
					var position = input.Position;
					position.X /= position.W;
					position.Y /= position.W;
					position.Z /= position.W;
					input.Position = position;
					outputs.Add(input);
				}
			}
			finally
			{
				outputs.CompleteAdding();
			}
		}
	}
}