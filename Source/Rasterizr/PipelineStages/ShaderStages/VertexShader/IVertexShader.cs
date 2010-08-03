namespace Rasterizr.PipelineStages.ShaderStages.VertexShader
{
	public interface IVertexShader
	{
		VertexShaderOutput Execute(VertexShaderInput vertexShaderInput);
	}
}