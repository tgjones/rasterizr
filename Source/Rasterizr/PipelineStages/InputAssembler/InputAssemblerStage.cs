using System.Collections;
using System.Collections.Generic;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader.VertexAttributes;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.VertexAttributes;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.InputAssembler
{
	public class InputAssemblerStage
	{
		public InputLayout InputLayout { get; set; }

		public IList Vertices { get; set; }
		//public IVertexShaderInputBuilder<TVertexInput, TVertexShaderInput> VertexShaderInputBuilder { get; set; }

		public void Process(IList<VertexShaderInput> outputs)
		{
			foreach (object input in Vertices)
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

		private static VertexAttribute CreateVertexAttribute(InputElementDescription inputElementDescription, object vertexInput)
		{
			return new VertexAttribute
			{
				Name = inputElementDescription.Name,
				InterpolationType = VertexAttributeInterpolationType.Linear,
				Value = VertexAttributeValueUtility.ToValue(inputElementDescription.Name, inputElementDescription.Format, vertexInput)
			};
		}
	}
}