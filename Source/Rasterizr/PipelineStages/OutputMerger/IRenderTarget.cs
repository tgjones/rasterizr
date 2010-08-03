using Nexus;

namespace Rasterizr.PipelineStages.OutputMerger
{
	public interface IRenderTarget
	{
		void Clear();

		Color GetPixel(int x, int y);
		void SetPixel(int x, int y, Color color);

		void BeginFrame();
		void EndFrame();
	}
}