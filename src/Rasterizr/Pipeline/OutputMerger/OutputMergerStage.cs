using System.Collections.Generic;
using Rasterizr.Diagnostics;
using Rasterizr.Pipeline.PixelShader;
using SlimShader;

namespace Rasterizr.Pipeline.OutputMerger
{
	public class OutputMergerStage
	{
		private readonly Device _device;
		private RenderTargetView[] _renderTargetViews;
		private DepthStencilView _depthStencilView;
	    private Number4 _blendFactor;

		public DepthStencilState DepthStencilState { get; set; }
		public int DepthStencilReference { get; set; }

		public BlendState BlendState { get; set; }
	    public Number4 BlendFactor
	    {
            get { return _blendFactor; }
            set { _blendFactor = value; }
	    }
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
				// TODO
				const int renderTargetIndex = 0;
				var renderTarget = _renderTargetViews[renderTargetIndex];

				// TODO
				const int renderTargetArrayIndex = 0;

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

					float newDepth = pixel.Samples[sampleIndex].Depth;
                    if (_depthStencilView != null)
					{
                        float currentDepth = _depthStencilView.GetDepth(renderTargetArrayIndex, pixel.X, pixel.Y, sampleIndex);
					    if (!DepthStencilState.DepthTestPasses(newDepth, currentDepth))
					    {
					        pixelHistoryEvent.ExclusionReason = PixelExclusionReason.FailedDepthTest;
					        _device.Loggers.AddPixelHistoryEvent(pixelHistoryEvent);
					        continue;
					    }
					}

					var source = pixel.Color;
					var destination = renderTarget.GetColor(renderTargetArrayIndex, pixel.X, pixel.Y, sampleIndex);

					// Use blend state to calculate final color.
					var finalColor = BlendState.DoBlend(renderTargetIndex, ref source, ref destination, ref _blendFactor);
					renderTarget.SetColor(renderTargetArrayIndex, pixel.X, pixel.Y, sampleIndex, ref finalColor);

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