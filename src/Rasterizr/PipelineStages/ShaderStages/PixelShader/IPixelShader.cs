using Nexus;
using Rasterizr.PipelineStages.Rasterizer;

namespace Rasterizr.PipelineStages.ShaderStages.PixelShader
{
	public interface IPixelShader
	{
		IPixelShaderInput BuildPixelShaderInput();
		ColorF Execute(Fragment fragment);
	}
}