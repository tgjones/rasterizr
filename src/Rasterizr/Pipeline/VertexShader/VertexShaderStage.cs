using System.Collections.Generic;
using Rasterizr.Pipeline.InputAssembler;

namespace Rasterizr.Pipeline.VertexShader
{
	public class VertexShaderStage : CommonShaderStage<VertexShader>
	{
		internal IEnumerable<VertexShaderOutput> Execute(IEnumerable<InputAssemblerVertexOutput> inputs)
		{
			foreach (var input in inputs)
			{
				yield return new VertexShaderOutput
				{
					VertexID = input.VertexID,
					InstanceID = input.InstanceID,
					
				};
			}
		}
	}
}