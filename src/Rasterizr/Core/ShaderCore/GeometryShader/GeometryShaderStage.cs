using System.Collections.Generic;
using Rasterizr.Core.ShaderCore.VertexShader;

namespace Rasterizr.Core.ShaderCore.GeometryShader
{
	public class GeometryShaderStage : PipelineStageBase<IVertexShaderOutput, IVertexShaderOutput>
	{
		public override void Run(List<IVertexShaderOutput> inputs, List<IVertexShaderOutput> outputs)
		{
			// TODO - implement programmable geometry shader.
			// For now just pass vertices through.
			for (int i = 0; i < inputs.Count; i += 3)
			{
				outputs.Add(inputs[i + 0]);
				outputs.Add(inputs[i + 1]);
				outputs.Add(inputs[i + 2]);
			}
		}
	}
}