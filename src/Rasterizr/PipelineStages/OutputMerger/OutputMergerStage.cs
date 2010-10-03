using System.Collections.Generic;
using Nexus;
using Rasterizr.PipelineStages.ShaderStages.PixelShader;
using Rasterizr.Util;

namespace Rasterizr.PipelineStages.OutputMerger
{
	public class OutputMergerStage
	{
		private IRenderTarget _renderTarget;

		public DepthBuffer DepthBuffer { get; private set; }

		public IRenderTarget RenderTarget
		{
			get { return _renderTarget; }
			set
			{
				_renderTarget = value;
				CreateDepthBuffer();
			}
		}

		public DepthStencilState DepthStencilState { get; private set; }
		public BlendState BlendState { get; set; }

		public OutputMergerStage()
		{
			DepthStencilState = new DepthStencilState();
			BlendState = new BlendState();
		}

		public void Process(IList<Pixel> inputs)
		{
			foreach (Pixel pixel in inputs)
			{
				if (!DepthTestPasses(pixel))
					continue;

				// Use blend state to calculate final color.
				ColorF finalColor;
				if (BlendState.BlendEnable)
					finalColor = BlendUtility.DoColorBlend(BlendState, pixel.Color,
						(ColorF) RenderTarget.GetPixel(pixel.X, pixel.Y));
				else
					finalColor = pixel.Color;

				RenderTarget.SetPixel(pixel.X, pixel.Y, (Color) finalColor);

				if (DepthStencilState.DepthEnable)
					DepthBuffer[pixel.X, pixel.Y] = pixel.Depth;
			}
		}

		private bool DepthTestPasses(Pixel pixel)
		{
			if (!DepthStencilState.DepthEnable)
				return true;

			return ComparisonUtility.DoComparison(DepthStencilState.DepthFunc,
			                                      pixel.Depth, DepthBuffer[pixel.X, pixel.Y]);
		}

		private void CreateDepthBuffer()
		{
			DepthBuffer = new DepthBuffer(_renderTarget.Width,_renderTarget.Height);
		}
	}
}