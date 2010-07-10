using Nexus;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader
{
	public interface IVertexShaderOutput
	{
		Point4D Position { get; set; }
	}
}