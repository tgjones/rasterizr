using System.ComponentModel.Composition;
using System.Windows.Controls;
using Rasterizr.Math;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.Rasterizer;
using Rasterizr.Platform.Wpf;
using Rasterizr.Resources;

namespace Rasterizr.SampleBrowser.Samples.BasicWindow
{
	[Export(typeof(SampleBase))]
	[ExportMetadata("SortOrder", 0)]
	public class BasicWindowSample : SampleBase
	{
		private DeviceContext _deviceContext;
		private RenderTargetView _renderTargetView;
		private WpfSwapChain _swapChain;

		public override string Name
		{
			get { return "Basic Window"; }
		}

		public override void Initialize(Image image)
		{
			const int width = 600;
			const int height = 400;

			// Create device and swap chain.
			var device = new Device();
			_swapChain = new WpfSwapChain(device, width, height);
			image.Source = _swapChain.Bitmap;

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
		}

		public override void Draw(DemoTime time)
		{
			_deviceContext.ClearRenderTargetView(_renderTargetView, new Color4F((float)(time.ElapsedTime / 5.0) % 1.0f, 0, 0, 1));
			_swapChain.Present();
		}
	}
}