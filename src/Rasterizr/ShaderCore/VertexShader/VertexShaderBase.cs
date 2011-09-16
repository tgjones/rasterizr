namespace Rasterizr.ShaderCore.VertexShader
{
	public abstract class VertexShaderBase<TVertexShaderInput, TVertexShaderOutput> : ShaderBase<TVertexShaderInput, TVertexShaderOutput>
		where TVertexShaderInput : new()
		where TVertexShaderOutput : IVertexShaderOutput, new() 
	{

	}
}