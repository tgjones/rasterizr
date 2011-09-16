using Nexus;

namespace Rasterizr.ShaderStages.PixelShader
{
	public abstract class PixelShaderBase<TPixelShaderInput> : ShaderBase<TPixelShaderInput, ColorF>
		where TPixelShaderInput : new()
	{
		
	}
}