using System.Collections.Generic;
using Nexus;
using Nexus.Graphics;
using Rasterizr.ShaderStages.PixelShader;

namespace Rasterizr.OutputMerger
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
			DepthStencilState = DepthStencilState.Default;
			BlendState = BlendState.Opaque;
		}

		public void Run(List<Pixel> inputs)
		{
			foreach (Pixel pixel in inputs)
			{
				for (int sampleIndex = 0; sampleIndex < pixel.Samples.Count; ++sampleIndex)
				{
					if (!pixel.Samples[sampleIndex].Covered)
						continue;

					float newDepth = pixel.Samples[sampleIndex].Depth;
					if (!DepthStencilState.DepthTestPasses(newDepth, DepthBuffer[pixel.X, pixel.Y, sampleIndex]))
						continue;

					// Use blend state to calculate final color.
					ColorF finalColor = BlendState.DoBlend(pixel.Color,
						RenderTarget[pixel.X, pixel.Y, sampleIndex]);

					RenderTarget[pixel.X, pixel.Y, sampleIndex] = finalColor;

					if (DepthStencilState.DepthWriteEnable)
						DepthBuffer[pixel.X, pixel.Y, sampleIndex] = newDepth;
				}
			}
		}
	}
}