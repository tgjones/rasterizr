using Nexus;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.ShaderStages.Core
{
	public interface ITextureImage
	{
		int Dimensions { get; }
		int GetBound(int dimension);

		ColorF GetColor(int x, int y);
	}
}