using System.Collections.Generic;

namespace Rasterizr.ShaderCore.VertexShader
{
	/// <summary>
	/// Applies a vertex program to the input vertices. The vertex program
	/// minimally computes the clip-space coordinates of the vertex
	/// positions and returns these as outputs to be used by the clipper
	/// and rasterizer.
	/// </summary>
	public class VertexShaderStage : PipelineStageBase<object, IVertexShaderOutput>
	{
		public IShader VertexShader { get; set; }

		public override void Run(List<object> inputs, List<IVertexShaderOutput> outputs)
		{
			if (VertexShader == null)
				throw new RasterizrException("VertexShader must be set");

			foreach (object input in inputs)
			{
				// Apply vertex shader.
				var vertexShaderOutput = (IVertexShaderOutput) VertexShader.Execute(input);
				outputs.Add(vertexShaderOutput);
			}
		}
	}

	// TODO: Implement cache for recently shaded vertices.
}