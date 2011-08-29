namespace Rasterizr.PipelineStages.ShaderStages.VertexShader
{
	public interface IVertexShader
	{
		IVertexShaderOutput Execute(object vertexShaderInput);
	}
}