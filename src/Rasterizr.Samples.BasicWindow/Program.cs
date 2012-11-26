using System;
using Rasterizr.Math;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.Rasterizer;
using Rasterizr.Platform.Wpf;
using Rasterizr.Resources;
using Rasterizr.Samples.Common;

namespace Rasterizr.Samples.BasicWindow
{
	public class Program : WpfDemoApp
	{
		private DeviceContext _deviceContext;
		private RenderTargetView _renderTargetView;
		private SwapChain _swapChain;

		protected override void Initialize(DemoConfiguration demoConfiguration)
		{
			// Create device and swap chain.
			var device = new Device();
			_swapChain = new WpfSwapChain(device, Image);
			_deviceContext = device.ImmediateContext;

			// Create RenderTargetView from the backbuffer.
			var backBuffer = Texture2D.FromSwapChain<Texture2D>(_swapChain, 0);
			_renderTargetView = new RenderTargetView(device, backBuffer);

			// Create DepthStencilView.
			var depthStencilBuffer = new Texture2D(device, new Texture2DDescription
			{
				Format = Format.D32_Float,
				ArraySize = 1,
				MipLevels = 1,
				Width = demoConfiguration.Width,
				Height = demoConfiguration.Height,
				BindFlags = BindFlags.DepthStencil
			});
			var depthStencilView = new DepthStencilView(device, depthStencilBuffer);

			// Prepare all the stages.
			_deviceContext.Rasterizer.SetViewports(new Viewport(0, 0, demoConfiguration.Width, demoConfiguration.Height, 0.0f, 1.0f));
			_deviceContext.OutputMerger.SetTargets(depthStencilView, _renderTargetView);
		}

		protected override void Draw(DemoTime time)
		{
			_deviceContext.ClearRenderTargetView(_renderTargetView, new Color4((byte) ((time.ElapsedTime * 30) % 255), 0, 0, 255));
			_swapChain.Present();

			base.Draw(time);
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			var program = new Program();
			program.Run(new DemoConfiguration("Rasterizr - Basic Window Sample") { WaitVerticalBlanking = true });
		}
	}
}
