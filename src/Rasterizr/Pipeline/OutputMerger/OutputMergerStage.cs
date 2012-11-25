using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	public class OutputMergerStage
	{
		private RenderTargetView[] _renderTargetViews;
		private DepthStencilView _depthStencilView;

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