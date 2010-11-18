using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nexus;
using Rasterizr.PipelineStages.ShaderStages.VertexShader;
using Rasterizr.VertexAttributes;

namespace Rasterizr.PipelineStages.InputAssembler
{
	public class InputAssemblerStage
	{
		public InputLayout InputLayout { get; set; }
		public PrimitiveTopology PrimitiveTopology { get; set; }

		public IList Vertices { get; set; }
		public Int32Collection Indices { get; set; }

		public void Process(IList<VertexShaderInput> outputs)
		{
			foreach (object input in GetVertices())
			{
				VertexShaderInput vertexShaderInput = new VertexShaderInput
				{
					Attributes = new VertexAttribute[InputLayout.Elements.Length]
				};
				for (int i = 0; i < InputLayout.Elements.Length; i++)
				{
					InputElementDescription inputElement = InputLayout.Elements[i];
					vertexShaderInput.Attributes[i] = CreateVertexAttribute(inputElement, input);
				}
				outputs.Add(vertexShaderInput);
			}
		}

		private IEnumerable<object> GetVertices()
		{
			if (Indices == null)
				return Vertices.Cast<object>();
			return GetIndexedVertices();
		}

		private IEnumerable<object> GetIndexedVertices()
		{
			for (int i = 0; i < Indices.Count; ++i)
				yield return Vertices[Indices[i]];
		}

		private static VertexAttribute CreateVertexAttribute(InputElementDescription inputElementDescription, object vertexInput)
		{
			return new VertexAttribute
			{
				Name = inputElementDescription.Name,
				InterpolationModifier = VertexAttributeInterpolationModifier.Linear,
				Value = VertexAttributeValueUtility.ToValue(inputElementDescription.Name, inputElementDescription.Format, vertexInput)
			};
		}
	}
}