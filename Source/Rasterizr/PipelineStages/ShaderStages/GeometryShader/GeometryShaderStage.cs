using System.Collections.Generic;
using Rasterizr.PipelineStages.PrimitiveAssembly;

namespace Rasterizr.PipelineStages.ShaderStages.GeometryShader
{
	public class GeometryShaderStage : PipelineStageBase<TrianglePrimitive, TrianglePrimitive>
	{
		public override void Process(IList<TrianglePrimitive> inputs, IList<TrianglePrimitive> outputs)
		{
			// TODO - implement programmable geometry shader.
			// For now just pass vertices through.
			for (int i = 0; i < inputs.Count; ++i)
				outputs.Add(inputs[i]);
		}
	}
}