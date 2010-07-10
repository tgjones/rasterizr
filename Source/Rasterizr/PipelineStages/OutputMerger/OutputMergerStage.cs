using System.Collections.Generic;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.PixelShader;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.OutputMerger
{
	public class OutputMergerStage
	{
		public IRenderTarget RenderTarget { get; set; }

		public void Process(IList<Pixel> inputs)
		{
			foreach (Pixel pixel in inputs)
				RenderTarget.SetPixel(pixel.X, pixel.Y, pixel.Color);
		}
	}
}