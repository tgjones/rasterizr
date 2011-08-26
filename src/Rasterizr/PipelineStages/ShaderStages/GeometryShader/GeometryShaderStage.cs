using System.Collections.Generic;
using Rasterizr.PipelineStages.ShaderStages.VertexShader;

namespace Rasterizr.PipelineStages.ShaderStages.GeometryShader
{
	public class GeometryShaderStage : PipelineStageBase<IVertexShaderOutput, IVertexShaderOutput>
	{
		public override void Process(IList<IVertexShaderOutput> inputs, IList<IVertexShaderOutput> outputs)
		{
			// TODO - implement programmable geometry shader.
			// For now just pass vertices through.
			for (int i = 0; i < inputs.Count; i += 3)
			{
				outputs.Add(inputs[i]);
				outputs.Add(inputs[i + 1]);
				outputs.Add(inputs[i + 2]);
			}
		}
	}
}