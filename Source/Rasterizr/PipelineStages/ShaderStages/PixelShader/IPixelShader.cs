using Nexus;
using Rasterizr.PipelineStages.Rasterizer;

namespace Rasterizr.PipelineStages.ShaderStages.PixelShader
{
	public interface IPixelShader
	{
		Color Execute(Fragment fragment);
	}
}