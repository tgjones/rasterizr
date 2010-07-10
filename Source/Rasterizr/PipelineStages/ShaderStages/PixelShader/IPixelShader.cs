using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.Rasterizer;
using Nexus;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.PixelShader
{
	public interface IPixelShader
	{
		Color Execute(Fragment fragment);
	}
}