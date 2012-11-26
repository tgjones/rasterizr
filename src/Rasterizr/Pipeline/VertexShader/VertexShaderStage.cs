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

				};
			}
		}
	}

	internal struct VertexShaderOutput
	{
		public int VertexID;
		public int InstanceID;
		public byte[] Data;
	}
}