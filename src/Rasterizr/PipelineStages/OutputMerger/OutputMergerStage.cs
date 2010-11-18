using System.Collections.Generic;
using Nexus;
using Nexus.Graphics;
using Rasterizr.PipelineStages.ShaderStages.PixelShader;
using Rasterizr.Util;

namespace Rasterizr.PipelineStages.OutputMerger
{
	public class OutputMergerStage
	{
		private RenderTargetView _renderTarget;

		public DepthBufferView DepthBuffer { get; private set; }

		public RenderTargetView RenderTarget
		{
			get { return _renderTarget; }
			set
			{
				_renderTarget = value;
				DepthBuffer = new DepthBufferView(new Surface<float>(_renderTarget.Width, _renderTarget.Height, _renderTarget.MultiSampleCount));
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
				for (int sampleIndex = 0; sampleIndex < pixel.Samples.Count; ++sampleIndex)
				{
					if (!pixel.Samples[sampleIndex].Covered)
						continue;

					if (!DepthTestPasses(pixel, sampleIndex))
						continue;

					// Use blend state to calculate final color.
					ColorF finalColor;
					if (BlendState.BlendEnable)
						finalColor = BlendUtility.DoColorBlend(BlendState, pixel.Color,
							RenderTarget[pixel.X, pixel.Y, sampleIndex]);
					else
						finalColor = pixel.Color;

					RenderTarget[pixel.X, pixel.Y, sampleIndex] = finalColor;

					if (DepthStencilState.DepthEnable)
						DepthBuffer[pixel.X, pixel.Y, sampleIndex] = pixel.Samples[sampleIndex].Depth;
				}
			}
		}

		private bool DepthTestPasses(Pixel pixel, int sampleIndex)
		{
			if (!DepthStencilState.DepthEnable)
				return true;

			return ComparisonUtility.DoComparison(DepthStencilState.DepthFunc,
				pixel.Samples[sampleIndex].Depth, DepthBuffer[pixel.X, pixel.Y, sampleIndex]);
		}
	}
}