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
			_renderTargetView = device.CreateRenderTargetView(backBuffer);

			// Create DepthStencilView.
			var depthStencilBuffer = device.CreateTexture2D(new Texture2DDescription
			{
				Format = Format.D32_Float,
				ArraySize = 1,
				MipLevels = 1,
				Width = demoConfiguration.Width,
				Height = demoConfiguration.Height,
				BindFlags = BindFlags.DepthStencil
			});
			var depthStencilView = device.CreateDepthStencilView(depthStencilBuffer);

			// Prepare all the stages.
			_deviceContext.Rasterizer.SetViewports(new Viewport(0, 0, demoConfiguration.Width, demoConfiguration.Height, 0.0f, 1.0f));
			_deviceContext.OutputMerger.SetTargets(depthStencilView, _renderTargetView);
		}

		protected override void Draw(DemoTime time)
		{
			_deviceContext.ClearRenderTargetView(_renderTargetView, new Color4F((float) (time.ElapsedTime / 5.0) % 1.0f, 0, 0, 1));
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
