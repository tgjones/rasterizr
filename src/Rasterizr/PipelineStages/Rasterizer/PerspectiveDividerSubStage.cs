using System.Collections.Concurrent;
using Rasterizr.PipelineStages.ShaderStages.VertexShader;

namespace Rasterizr.PipelineStages.Rasterizer
{
	public class PerspectiveDividerSubStage : RasterizerSubStageBase<IVertexShaderOutput, IVertexShaderOutput>
	{
		public override void Run(BlockingCollection<IVertexShaderOutput> inputs, BlockingCollection<IVertexShaderOutput> outputs)
		{
			foreach (var input in inputs)
			{
				var position = input.Position;
				position.X /= position.W;
				position.Y /= position.W;
				position.Z /= position.W;
				input.Position = position;
				outputs.Add(input);
			}
			outputs.CompleteAdding();
		}
	}
}