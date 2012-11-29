using System.Collections.Generic;
using System.Diagnostics;
using Rasterizr.Math;
using Rasterizr.Pipeline.PixelShader;
using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	public class OutputMergerStage
	{
		private RenderTargetView[] _renderTargetViews;
		private DepthStencilView _depthStencilView;

		public BlendState BlendState { get; set; }
		public Color4F BlendFactor { get; set; }
		public int BlendSampleMask { get; set; }

		internal int MultiSampleCount
		{
			// TODO
			get { return 1; }
		}

		public OutputMergerStage(Device device)
		{
			BlendState = new BlendState(device, BlendStateDescription.Default);
		}

		public void GetTargets(out DepthStencilView depthStencilView, out RenderTargetView[] renderTargetViews)
		{
			depthStencilView = _depthStencilView;
			renderTargetViews = _renderTargetViews;
		}

		public void SetTargets(DepthStencilView depthStencilView, params RenderTargetView[] renderTargetViews)
		{
			_depthStencilView = depthStencilView;
			_renderTargetViews = renderTargetViews;
		}

		internal void Execute(IEnumerable<Pixel> inputs)
		{
			// TODO
			const int renderTargetIndex = 0;
			var renderTarget = _renderTargetViews[renderTargetIndex];
			foreach (var pixel in inputs)
			{
				for (int sampleIndex = 0; sampleIndex < MultiSampleCount; ++sampleIndex)
				{
					//if (!pixel.Samples[sampleIndex].Covered)
					//    continue;

					float newDepth = pixel.Samples[sampleIndex].Depth;
					// TODO
					//if (!DepthStencilState.DepthTestPasses(newDepth, DepthBuffer[pixel.X, pixel.Y, sampleIndex]))
					//    continue;

					// Use blend state to calculate final color.
					Color4F finalColor = BlendState.DoBlend(renderTargetIndex, pixel.Color,
						renderTarget[pixel.X, pixel.Y, sampleIndex],
						BlendFactor);

					renderTarget[pixel.X, pixel.Y, sampleIndex] = finalColor;

					// TODO
					//if (DepthStencilState.DepthWriteEnable)
					//    DepthBuffer[pixel.X, pixel.Y, sampleIndex] = newDepth;
				}
			}
			renderTarget.Invalidate();
		}
	}
}