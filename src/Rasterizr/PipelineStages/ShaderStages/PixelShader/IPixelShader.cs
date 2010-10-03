using Nexus;
using Rasterizr.PipelineStages.Rasterizer;

namespace Rasterizr.PipelineStages.ShaderStages.PixelShader
{
	public interface IPixelShader
	{
		ColorF Execute(Fragment fragment);
	}
}