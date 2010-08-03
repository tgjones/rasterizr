using System.Collections.Generic;
using Rasterizr.PipelineStages.ShaderStages.PixelShader;

namespace Rasterizr.PipelineStages.OutputMerger
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