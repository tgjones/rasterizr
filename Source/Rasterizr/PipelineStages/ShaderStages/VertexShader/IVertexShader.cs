namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader
{
	public interface IVertexShader
	{
		VertexShaderOutput Execute(VertexShaderInput vertexShaderInput);
	}
}