using System;
using System.Collections.Generic;
using System.Reflection;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader.VertexAttributes;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.ShaderCore;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.VertexAttributes;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader
{
	public abstract class VertexShaderBase<TVertexShaderInput, TVertexShaderOutput> : ShaderBase, IVertexShader
		where TVertexShaderInput : new()
		where TVertexShaderOutput : IVertexShaderOutput, new()
	{
		public abstract TVertexShaderOutput Execute(TVertexShaderInput vertexShaderInput);

		public VertexShaderOutput Execute(VertexShaderInput vertexShaderInput)
		{
			// Convert VertexShaderInput to TVertexShaderInput.
			TVertexShaderInput input = BuildTypedVertexShaderInput(vertexShaderInput);

			// Execute VertexShader.
			TVertexShaderOutput output = Execute(input);

			// Convert TVertexShaderOutput to VertexShaderOutput.
			return BuildVertexShaderOutput(output);
		}

		private static TVertexShaderInput BuildTypedVertexShaderInput(VertexShaderInput vertexShaderInput)
		{
			TVertexShaderInput typedInput = new TVertexShaderInput();
			object wrapper = typedInput;
			foreach (VertexAttribute vertexAttribute in vertexShaderInput.Attributes)
			{
				FieldInfo propertyInfo = typeof(TVertexShaderInput).GetField(vertexAttribute.Name);
				//TypedReference typedReference = TypedReference.MakeTypedReference(typedInput, new[] { propertyInfo });
				//propertyInfo.SetValueDirect(typedReference, vertexAttribute.Value.Value);
				propertyInfo.SetValue(wrapper, vertexAttribute.Value.Value);
			}
			typedInput = (TVertexShaderInput) wrapper;
			return typedInput;
		}

		private static VertexShaderOutput BuildVertexShaderOutput(TVertexShaderOutput vertexShaderOutput)
		{
			VertexShaderOutput output = new VertexShaderOutput { Position = vertexShaderOutput.Position };
			List<VertexAttribute> attributes = new List<VertexAttribute>();
			foreach (FieldInfo field in typeof(TVertexShaderOutput).GetFields())
			{
				if (field.Name != "Position")
				{
					attributes.Add(new VertexAttribute
					{
						Name = field.Name,
						InterpolationType = VertexAttributeInterpolationType.Perspective,
						Value = VertexAttributeValueUtility.ToValue(field.Name, field.FieldType, vertexShaderOutput)
					});
				}
			}
			output.Attributes = attributes.ToArray();
			return output;
		}
	}
}