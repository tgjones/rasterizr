using System.Collections.Generic;

namespace Rasterizr.PipelineStages.ShaderStages.VertexShader
{
	/// <summary>
	/// Applies a vertex program to the input vertices. The vertex program
	/// minimally computes the clip-space coordinates of the vertex
	/// positions and returns these as outputs to be used by the clipper
	/// and rasterizer.
	/// </summary>
	public class VertexShaderStage : PipelineStageBase<IVertex, IVertexShaderOutput>
	{
		public IVertexShader VertexShader { get; set; }

		public override void Process(IList<IVertex> inputs, IList<IVertexShaderOutput> outputs)
		{
			foreach (IVertex input in inputs)
			{
				// Apply vertex shader.
				IVertexShaderOutput vertexShaderOutput = VertexShader.Execute(input);
				outputs.Add(vertexShaderOutput);
			}
		}

		// TODO: Implement cache for recently shaded vertices.
	}
}