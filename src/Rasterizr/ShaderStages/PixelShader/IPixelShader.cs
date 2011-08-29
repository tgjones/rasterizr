using Nexus;
using Rasterizr.Rasterizer;

namespace Rasterizr.ShaderStages.PixelShader
{
	public interface IPixelShader
	{
		IPixelShaderInput BuildPixelShaderInput();
		ColorF Execute(Fragment fragment);
	}
}