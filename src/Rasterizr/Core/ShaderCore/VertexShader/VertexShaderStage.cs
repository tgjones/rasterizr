using System.Collections.Generic;
using System.Linq;
using Nexus;

namespace Rasterizr.Core.ShaderCore.VertexShader
{
	/// <summary>
	/// Applies a vertex program to the input vertices. The vertex program
	/// minimally computes the clip-space coordinates of the vertex
	/// positions and returns these as outputs to be used by the clipper
	/// and rasterizer.
	/// </summary>
	public class VertexShaderStage : PipelineStageBase<object, TransformedVertex>
	{
		public IShader VertexShader { get; set; }

		public override IEnumerable<TransformedVertex> Run(IEnumerable<object> inputs)
		{
			if (VertexShader == null)
				throw new RasterizrException("VertexShader must be set");

			var vertexShaderDescription = ShaderDescriptionCache.GetDescription(VertexShader);
			var positionOutputParameter =
				vertexShaderDescription.GetOutputParameterBySemantic(new Semantic(SystemValueType.Position));
			if (positionOutputParameter == null)
				throw new RasterizrException("VertexShader output must include a field with the Position system value semantic");
			if (positionOutputParameter.ParameterType != typeof(Point4D))
				throw new RasterizrException("VertexShader output position must be of type Nexus.Point4D");

			return inputs.Select(input =>
			{
				var shaderOutput = VertexShader.Execute(input);
				return new TransformedVertex(shaderOutput, (Point4D) positionOutputParameter.GetValue(shaderOutput));
			});
		}
	}

	// TODO: Implement cache for recently shaded vertices.
}