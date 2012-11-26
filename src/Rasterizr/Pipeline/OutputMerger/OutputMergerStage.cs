using Rasterizr.Math;
using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	public class OutputMergerStage
	{
		private RenderTargetView[] _renderTargetViews;
		private DepthStencilView _depthStencilView;

		public BlendState BlendState { get; set; }
		public Color4 BlendFactor { get; set; }
		public int BlendSampleMask { get; set; }

		internal int MultiSampleCount
		{
			// TODO
			get { return 1; }
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
	}
}