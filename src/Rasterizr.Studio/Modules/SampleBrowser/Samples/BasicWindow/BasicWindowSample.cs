using System.ComponentModel.Composition;
using System.Windows.Media.Imaging;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.Rasterizer;
using Rasterizr.Platform.Wpf;
using Rasterizr.Resources;

namespace Rasterizr.Studio.Modules.SampleBrowser.Samples.BasicWindow
{
	[Export(typeof(SampleBase))]
	[ExportMetadata("SortOrder", 0)]
	public class BasicWindowSample : SampleBase
	{
		private DeviceContext _deviceContext;
		private RenderTargetView _renderTargetView;
		private SwapChain _swapChain;

		public override string Name
		{
			get { return "Basic Window"; }
		}

        public override WriteableBitmap Initialize(Device device)
		{
			const int width = 600;
			const int height = 400;

			// Create device and swap chain.
            var swapChainPresenter = new WpfSwapChainPresenter();
			_swapChain = device.CreateSwapChain(width, height, swapChainPresenter);

			_deviceContext = device.ImmediateContext;

			// Create RenderTargetView from the backbuffer.
			var backBuffer = Texture2D.FromSwapChain(_swapChain, 0);
			_renderTargetView = device.CreateRenderTargetView(backBuffer);

			// Create DepthStencilView.
			var depthStencilBuffer = device.CreateTexture2D(new Texture2DDescription
			{
				ArraySize = 1,
				MipLevels = 1,
				Width = width,
				Height = height,
				BindFlags = BindFlags.DepthStencil
			});
			var depthStencilView = device.CreateDepthStencilView(depthStencilBuffer);

			// Prepare all the stages.
			_deviceContext.Rasterizer.SetViewports(new Viewport(0, 0, width, height, 0.0f, 1.0f));
			_deviceContext.OutputMerger.SetTargets(depthStencilView, _renderTargetView);

            return swapChainPresenter.Bitmap;
		}

        public override void Draw(float time)
		{
            _deviceContext.ClearRenderTargetView(_renderTargetView, new Color4((float) (time / 5.0) % 1.0f, 0, 0, 1));
			_deviceContext.Present(_swapChain);
		}
	}
}