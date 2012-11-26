using System.Collections.Generic;
using System.Linq;
using Rasterizr.Math;
using Rasterizr.Pipeline.InputAssembler;
using SlimShader;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Pipeline.VertexShader
{
	public class VertexShaderStage : CommonShaderStage<VertexShader>
	{
		internal IEnumerable<VertexShaderOutput> Execute(IEnumerable<InputAssemblerVertexOutput> inputs)
		{
			foreach (var input in inputs)
			{
				var outputs = new Number4[Shader.Bytecode.OutputSignature.Parameters.Count];
				ExecuteShader(input.Data, outputs);

				var positionParameter = Shader.Bytecode.OutputSignature.Parameters.Single(x => x.SystemValueType == Name.Position);

				yield return new VertexShaderOutput
				{
					VertexID = input.VertexID,
					InstanceID = input.InstanceID,
					Position = Vector4.FromNumber4(outputs[positionParameter.Register]),
					Data = outputs
				};
			}
		}
	}
}