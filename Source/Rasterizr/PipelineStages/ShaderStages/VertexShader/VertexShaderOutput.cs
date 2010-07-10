using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader.VertexAttributes;
using Nexus;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader
{
	public struct VertexShaderOutput
	{
		public Point4D Position;
		public VertexAttribute[] Attributes;
	}
}