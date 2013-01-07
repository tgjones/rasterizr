using System.Collections.Generic;
using Rasterizr.Diagnostics;
using Rasterizr.Math;
using Rasterizr.Pipeline.PixelShader;

namespace Rasterizr.Pipeline.OutputMerger
{
	public class OutputMergerStage
	{
		private readonly Device _device;
		private RenderTargetView[] _renderTargetViews;
		private DepthStencilView _depthStencilView;

		public DepthStencilState DepthStencilState { get; set; }
		public int DepthStencilReference { get; set; }

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
			_device = device;
			DepthStencilState = new DepthStencilState(device, DepthStencilStateDescription.Default);
			BlendState = new BlendState(device, BlendStateDescription.Default);
		}

		public void GetTargets(out DepthStencilView depthStencilView, out RenderTargetView[] renderTargetViews)
		{
			depthStencilView = _depthStencilView;
			renderTargetViews = _renderTargetViews;
		}

		public void SetTargets(DepthStencilView depthStencilView, params RenderTargetView[] renderTargetViews)
		{
			_device.Loggers.BeginOperation(OperationType.OutputMergerStageSetTargets, depthStencilView, renderTargetViews);
			_depthStencilView = depthStencilView;
			_renderTargetViews = renderTargetViews;
		}

		internal void Execute(IEnumerable<Pixel> inputs)
		{
			foreach (var pixel in inputs)
			{
				for (int sampleIndex = 0; sampleIndex < MultiSampleCount; ++sampleIndex)
				{
					if (!pixel.Samples[sampleIndex].Covered)
						continue;

					var pixelHistoryEvent = new DrawEvent
					{
						PrimitiveID = pixel.PrimitiveID,
						X = pixel.X,
						Y = pixel.Y,
						PixelShader = pixel.Color
					};

					// TODO
					const int renderTargetIndex = 0;
					var renderTarget = _renderTargetViews[renderTargetIndex];

					// TODO
					const int renderTargetArrayIndex = 0;

					float newDepth = pixel.Samples[sampleIndex].Depth;
					if (_depthStencilView != null && !DepthStencilState.DepthTestPasses(newDepth, 
						_depthStencilView.GetDepth(renderTargetArrayIndex, pixel.X, pixel.Y, sampleIndex)))
					{
						pixelHistoryEvent.ExclusionReason = PixelExclusionReason.FailedDepthTest;
						_device.Loggers.AddPixelHistoryEvent(pixelHistoryEvent);
						continue;
					}

					// Clamp pixel colour to range supported by render target format.
					var source = FormatHelper.Clamp(pixel.Color, renderTarget.ActualFormat);
					var destination = renderTarget.GetColor(renderTargetArrayIndex, pixel.X, pixel.Y, sampleIndex);

					// Use blend state to calculate final color.
					Color4F finalColor = BlendState.DoBlend(renderTargetIndex, source, destination, BlendFactor);
					renderTarget.SetColor(renderTargetArrayIndex, pixel.X, pixel.Y, sampleIndex, finalColor);

					pixelHistoryEvent.Previous = destination;
					pixelHistoryEvent.Result = finalColor;
					_device.Loggers.AddPixelHistoryEvent(pixelHistoryEvent);

					if (_depthStencilView != null && DepthStencilState.Description.IsDepthEnabled)
						_depthStencilView.SetDepth(renderTargetArrayIndex, pixel.X, pixel.Y, sampleIndex, newDepth);
				}
			}
		}
	}
}