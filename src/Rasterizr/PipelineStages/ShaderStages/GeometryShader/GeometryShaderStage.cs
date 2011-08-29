using System.Collections.Concurrent;
using Rasterizr.PipelineStages.ShaderStages.VertexShader;

namespace Rasterizr.PipelineStages.ShaderStages.GeometryShader
{
	public class GeometryShaderStage : PipelineStageBase<IVertexShaderOutput, IVertexShaderOutput>
	{
		public override void Run(BlockingCollection<IVertexShaderOutput> inputs, BlockingCollection<IVertexShaderOutput> outputs)
		{
			// TODO - implement programmable geometry shader.
			// For now just pass vertices through.
			var inputsEnumerator = inputs.GetConsumingEnumerable().GetEnumerator();
			while (inputsEnumerator.MoveNext())
			{
				outputs.Add(inputsEnumerator.Current);
				inputsEnumerator.MoveNext();
				outputs.Add(inputsEnumerator.Current);
				inputsEnumerator.MoveNext();
				outputs.Add(inputsEnumerator.Current);
			}
			outputs.CompleteAdding();
		}
	}
}