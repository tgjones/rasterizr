using System.Collections.Generic;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader
{
	/// <summary>
	/// Applies a vertex program to the input vertices. The vertex program
	/// minimally computes the clip-space coordinates of the vertex
	/// positions and returns these as outputs to be used by the clipper
	/// and rasterizer.
	/// </summary>
	public class VertexShaderStage : PipelineStageBase<VertexShaderInput, VertexShaderOutput>
	{
		public IVertexShader VertexShader { get; set; }

		public override void Process(IList<VertexShaderInput> inputs, IList<VertexShaderOutput> outputs)
		{
			foreach (VertexShaderInput input in inputs)
			{
				// Apply vertex shader.
				VertexShaderOutput vertexShaderOutput = VertexShader.Execute(input);
				outputs.Add(vertexShaderOutput);
			}
		}

		// TODO: Implement cache for recently shaded vertices.
	}
}