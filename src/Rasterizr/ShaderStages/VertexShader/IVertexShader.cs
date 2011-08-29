namespace Rasterizr.ShaderStages.VertexShader
{
	public interface IVertexShader
	{
		IVertexShaderOutput Execute(object vertexShaderInput);
	}
}