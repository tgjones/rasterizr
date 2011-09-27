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
		private IShader _vertexShader;
		private SignatureParameterDescription _positionOutputParameter;

		public IShader VertexShader
		{
			get { return _vertexShader; }
			set
			{
				_vertexShader = value;
				var vertexShaderDescription = ShaderDescriptionCache.GetDescription(VertexShader);
				_positionOutputParameter = vertexShaderDescription.GetOutputParameterBySemantic(new Semantic(SystemValueType.Position));
				if (_positionOutputParameter == null)
					throw new RasterizrException("VertexShader output must include a field with the Position system value semantic");
				if (_positionOutputParameter.ParameterType != typeof(Point4D))
					throw new RasterizrException("VertexShader output position must be of type Nexus.Point4D");
			}
		}

		public override IEnumerable<TransformedVertex> Run(IEnumerable<object> inputs)
		{
			if (VertexShader == null)
				throw new RasterizrException("VertexShader must be set");

			return inputs.Select(input =>
			{
				var shaderOutput = VertexShader.Execute(input);
				return new TransformedVertex(shaderOutput, (Point4D) _positionOutputParameter.GetValue(shaderOutput));
			});
		}
	}

	// TODO: Implement cache for recently shaded vertices.
}