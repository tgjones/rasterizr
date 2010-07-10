using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.TriangleSetup;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.PixelShader
{
	public interface IPixelShaderInputBuilder<out TPixelShaderInput>
	{
		TPixelShaderInput Build(InterpolatedVertexAttribute[] attributes);
	}
}