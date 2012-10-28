using System.Collections.Generic;

namespace Rasterizr.ShaderCore.GeometryShader
{
	public class GeometryShaderStage : PipelineStageBase<TransformedVertex, TransformedVertex>
	{
		public override IEnumerable<TransformedVertex> Run(IEnumerable<TransformedVertex> inputs)
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