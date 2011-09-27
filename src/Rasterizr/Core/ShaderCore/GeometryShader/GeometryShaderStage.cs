using System.Collections.Generic;
using Rasterizr.Core.ShaderCore.VertexShader;

namespace Rasterizr.Core.ShaderCore.GeometryShader
{
	public class GeometryShaderStage : PipelineStageBase<IVertexShaderOutput, IVertexShaderOutput>
	{
		public override IEnumerable<IVertexShaderOutput> Run(IEnumerable<IVertexShaderOutput> inputs)
		{
			// TODO - implement programmable geometry shader.
			// For now just pass vertices through.
			var enumerator = inputs.GetEnumerator();
			while (enumerator.MoveNext())
			{
				yield return enumerator.Current;
				enumerator.MoveNext();
				yield return enumerator.Current;
				enumerator.MoveNext();
				yield return enumerator.Current;
			}
		}
	}
}