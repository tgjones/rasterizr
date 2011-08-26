namespace Rasterizr.PipelineStages.ShaderStages.VertexShader
{
	public interface IVertexShader
	{
		IVertexShaderOutput Execute(IVertex vertexShaderInput);
	}
}