using Nexus;
using Rasterizr.Rasterizer;

namespace Rasterizr.ShaderStages.PixelShader
{
	public interface IPixelShader
	{
		object BuildPixelShaderInput();
		ColorF Execute(Fragment fragment);
	}
}