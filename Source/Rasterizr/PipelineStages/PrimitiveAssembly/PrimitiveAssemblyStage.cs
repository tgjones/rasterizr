using System.Collections.Generic;
using Rasterizr.PipelineStages.ShaderStages.VertexShader;

namespace Rasterizr.PipelineStages.PrimitiveAssembly
{
	public class PrimitiveAssemblyStage : PipelineStageBase<VertexShaderOutput, TrianglePrimitive>
	{
		public override void Process(IList<VertexShaderOutput> inputs, IList<TrianglePrimitive> outputs)
		{
			for (int i = 0; i < inputs.Count; i += 3)
				outputs.Add(new TrianglePrimitive(inputs[i], inputs[i + 1], inputs[i + 2]));
		}
	}
}