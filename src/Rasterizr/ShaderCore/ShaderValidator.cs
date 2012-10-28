using System.Linq;
using Rasterizr.InputAssembler;

namespace Rasterizr.ShaderCore
{
	/// <summary>
	/// Ensures that the output of an upstream shader is 
	/// compatible with the input of the downstream shader.
	/// </summary>
	public class ShaderValidator
	{
		public void CheckCompatibility(IShader upstream, IShader downstream)
		{
			// Check that all inputs of downstream shader are either:
			// (a) present in upstream shader, or
			// (b) system values valid for this type of shader
			// TODO

			var upstreamDescription = ShaderDescriptionCache.GetDescription(upstream);
			var downstreamDescription = ShaderDescriptionCache.GetDescription(downstream);

			foreach (var downstreamInput in downstreamDescription.InputParameters)
				if (!upstreamDescription.OutputParameters.Any(spd => spd.Semantic == downstreamInput.Semantic))
					throw new RasterizrException(
						"Downstream shader of type '{0}' is not compatible with upstream shader of type '{1}' because the downstream shader includes a property with semantic '{2}' that is not supplied by the upstream shader.",
						downstream.GetType(), upstream.GetType(), downstreamInput.Semantic);
		}

		public void CheckCompatibility(InputLayout upstream, IShader downstream)
		{
			// Check that all inputs of downstream shader are either:
			// (a) present in input layout, or
			// (b) system values valid for this type of shader
			// TODO

			var downstreamDescription = ShaderDescriptionCache.GetDescription(downstream);

			foreach (var downstreamInput in downstreamDescription.InputParameters)
				if (!upstream.Elements.Any(ied => ied.SemanticName == downstreamInput.Semantic.Name && ied.SemanticIndex == downstreamInput.Semantic.Index))
					throw new RasterizrException(
						"Vertex shader of type '{0}' is not compatible with input layout because the vertex shader includes a property with semantic '{1}' that is not supplied by the input layout.",
						downstream.GetType(), downstreamInput.Semantic);
		}
	}
}