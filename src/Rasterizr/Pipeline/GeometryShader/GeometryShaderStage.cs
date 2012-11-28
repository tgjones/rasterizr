using System.Collections.Generic;
using Rasterizr.Pipeline.InputAssembler;

namespace Rasterizr.Pipeline.GeometryShader
{
	public class GeometryShaderStage : CommonShaderStage<GeometryShader>
	{
		internal IEnumerable<GeometryShaderOutput> Execute(IEnumerable<InputAssemblerPrimitiveOutput> inputs)
		{
			foreach (var input in inputs)
			{
				if (Shader != null)
				{
					//var outputs = new Vector4[Shader.Bytecode.OutputSignature.Parameters.Count];
					//ExecuteShader(input.Data, outputs);

					//var positionParameter = Shader.Bytecode.OutputSignature.Parameters.Single(x => x.SystemValueType == Name.Position);	
				}
				else
				{
					yield return new GeometryShaderOutput
					{
						PrimitiveType = input.PrimitiveType,
						Vertices = input.Vertices
					};
				}
			}
		}
	}
}