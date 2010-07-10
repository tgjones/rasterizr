namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader.VertexAttributes
{
	public interface IVertexAttributeBuilder<in TVertexShaderOutput>
	{
		VertexAttribute[] Build(TVertexShaderOutput vertexShaderOutput);
	}
}