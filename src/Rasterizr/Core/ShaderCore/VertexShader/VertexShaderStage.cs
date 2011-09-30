using System.Collections.Generic;
using System.Linq;
using Nexus;
using Rasterizr.Core.InputAssembler;

namespace Rasterizr.Core.ShaderCore.VertexShader
{
	/// <summary>
	/// Applies a vertex program to the input vertices. The vertex program
	/// minimally computes the clip-space coordinates of the vertex
	/// positions and returns these as outputs to be used by the clipper
	/// and rasterizer.
	/// </summary>
	public class VertexShaderStage : PipelineStageBase<InputAssemblerOutput, TransformedVertex>
	{
		private readonly InputAssemblerStage _inputAssemblerStage;
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

		public VertexShaderStage(InputAssemblerStage inputAssemblerStage)
		{
			_inputAssemblerStage = inputAssemblerStage;
		}

		public override IEnumerable<TransformedVertex> Run(IEnumerable<InputAssemblerOutput> inputs)
		{
			if (VertexShader == null)
				throw new RasterizrException("VertexShader must be set");

			return inputs.Select(input =>
			{
				var shaderInput = CreateVertexShaderInput(input);
				var shaderOutput = VertexShader.Execute(shaderInput);
				return new TransformedVertex(shaderOutput, (Point4D) _positionOutputParameter.GetValue(shaderOutput));
			});
		}

		private object CreateVertexShaderInput(InputAssemblerOutput input)
		{
			var vertexShaderInput = VertexShader.BuildShaderInput();

			// Calculate interpolated attribute values for this fragment.
			var inputLayoutDescription = InputLayoutDescriptionCache.GetDescription(_inputAssemblerStage.InputLayout,
				input.Vertex.GetType());
			var vertexShaderDescription = ShaderDescriptionCache.GetDescription(VertexShader);
			foreach (var property in vertexShaderDescription.InputParameters)
			{
				var value = inputLayoutDescription.GetValue(input.Vertex, property.Semantic);

				// Set value onto pixel shader input.
				property.SetValue(ref vertexShaderInput, value);

				// TODO: Do something different if input parameter is a system value.
			}
			return vertexShaderInput;
		}
	}

	// TODO: Implement cache for recently shaded vertices.
}