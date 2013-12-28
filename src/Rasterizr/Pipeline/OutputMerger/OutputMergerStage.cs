using System.Collections.Generic;
using Rasterizr.Diagnostics;
using Rasterizr.Pipeline.PixelShader;
using Rasterizr.Util;
using SlimShader;

namespace Rasterizr.Pipeline.OutputMerger
{
	public class OutputMergerStage
	{
        public event DiagnosticEventHandler SettingDepthStencilState;
        public event DiagnosticEventHandler SettingDepthStencilReference;
        public event DiagnosticEventHandler SettingBlendState;
        public event DiagnosticEventHandler SettingBlendFactor;
        public event DiagnosticEventHandler SettingBlendSampleMask;
        public event DiagnosticEventHandler SettingTargets;

	    internal event PixelEventHandler ProcessedPixel;

		private RenderTargetView[] _renderTargetViews;
		private DepthStencilView _depthStencilView;
        private Color4 _blendFactor;
        private Number4 _blendFactorNumber;
	    private DepthStencilState _depthStencilState;
	    private int _depthStencilReference;
	    private BlendState _blendState;
	    private int _blendSampleMask;

	    public DepthStencilState DepthStencilState
	    {
	        get { return _depthStencilState; }
	        set
	        {
                DiagnosticUtilities.RaiseEvent(this, SettingDepthStencilState, DiagnosticUtilities.GetID(value));
	            _depthStencilState = value;
	        }
	    }

	    public int DepthStencilReference
	    {
	        get { return _depthStencilReference; }
	        set
	        {
                DiagnosticUtilities.RaiseEvent(this, SettingDepthStencilReference, value);
	            _depthStencilReference = value;
	        }
	    }

	    public BlendState BlendState
	    {
	        get { return _blendState; }
	        set
	        {
                DiagnosticUtilities.RaiseEvent(this, SettingBlendState, DiagnosticUtilities.GetID(value));
	            _blendState = value;
	        }
	    }

	    public Color4 BlendFactor
	    {
            get { return _blendFactor; }
	        set
	        {
                DiagnosticUtilities.RaiseEvent(this, SettingBlendFactor, value);
	            _blendFactor = value;
	            _blendFactorNumber = value.ToNumber4();
	        }
	    }

	    public int BlendSampleMask
	    {
	        get { return _blendSampleMask; }
	        set
	        {
                DiagnosticUtilities.RaiseEvent(this, SettingBlendSampleMask, value);
	            _blendSampleMask = value;
	        }
	    }

	    internal int MultiSampleCount
		{
			// TODO
			get { return 1; }
		}

		public OutputMergerStage(Device device)
		{
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
            DiagnosticUtilities.RaiseEvent(this, SettingTargets, 
                DiagnosticUtilities.GetID(depthStencilView),
                DiagnosticUtilities.GetIDs(renderTargetViews));
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

				var renderTargetArrayIndex = pixel.RenderTargetArrayIndex;

			    var processedPixelHandler = ProcessedPixel;

				for (int sampleIndex = 0; sampleIndex < MultiSampleCount; ++sampleIndex)
				{
					if (!pixel.Samples[sampleIndex].Covered)
						continue;

					float newDepth = pixel.Samples[sampleIndex].Depth;

                    var source = pixel.Color;
                    var destination = renderTarget.GetColor(renderTargetArrayIndex, pixel.X, pixel.Y, sampleIndex);
				    
                    if (_depthStencilView != null)
					{
                        float currentDepth = _depthStencilView.GetDepth(renderTargetArrayIndex, pixel.X, pixel.Y, sampleIndex);
					    if (!DepthStencilState.DepthTestPasses(newDepth, currentDepth))
					    {
					        if (processedPixelHandler != null)
					            processedPixelHandler(this, new PixelEventArgs(
					                pixel.Vertices, pixel.PrimitiveID, renderTargetArrayIndex, pixel.X, pixel.Y,
                                    ref source, ref destination, null,
					                PixelExclusionReason.FailedDepthTest));
					        continue;
					    }
					}
					
					// Use blend state to calculate final color.
                    var finalColor = BlendState.DoBlend(renderTargetIndex, ref source, ref destination, ref _blendFactorNumber);
				    finalColor = Number4.Saturate(ref finalColor);
					renderTarget.SetColor(renderTargetArrayIndex, pixel.X, pixel.Y, sampleIndex, ref finalColor);

				    if (processedPixelHandler != null)
				        processedPixelHandler(this, new PixelEventArgs(
				            pixel.Vertices, pixel.PrimitiveID, renderTargetArrayIndex, pixel.X, pixel.Y,
				            ref source, ref destination, finalColor,
				            PixelExclusionReason.NotExcluded));

					if (_depthStencilView != null && DepthStencilState.Description.IsDepthEnabled)
						_depthStencilView.SetDepth(renderTargetArrayIndex, pixel.X, pixel.Y, sampleIndex, newDepth);
				}
			}
		}
	}
}