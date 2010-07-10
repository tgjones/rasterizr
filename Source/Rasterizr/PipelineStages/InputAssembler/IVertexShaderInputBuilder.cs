namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.InputAssembler
{
	public interface IVertexShaderInputBuilder<in TVertexInput, out TVertexShaderInput>
	{
		TVertexShaderInput Build(TVertexInput vertexInput);
	}
}