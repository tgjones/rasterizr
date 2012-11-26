using System;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.PixelShader;
using Rasterizr.Pipeline.Rasterizer;
using Rasterizr.Pipeline.VertexShader;
using Rasterizr.Platform.Wpf;
using Rasterizr.Resources;
using Rasterizr.Samples.Common;
using SharpDX;
using SlimShader.Compiler;
using Buffer = Rasterizr.Resources.Buffer;

namespace Rasterizr.Samples.BasicTriangle
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

			// Compile Vertex and Pixel shaders
			var vertexShaderByteCode = ShaderCompiler.CompileFromFile("MiniTri.fx", "VS", "vs_4_0");
			var vertexShader = new VertexShader(device, vertexShaderByteCode);

			var pixelShaderByteCode = ShaderCompiler.CompileFromFile("MiniTri.fx", "PS", "ps_4_0");
			var pixelShader = new PixelShader(device, pixelShaderByteCode);

			// Layout from VertexShader input signature
			var layout = new InputLayout(device,
				vertexShaderByteCode.InputSignature,
				new[]
				{
					new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0),
					new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 0)
				});

			// Instantiate Vertex buffer from vertex data
			var vertices = Buffer.Create(device, BindFlags.VertexBuffer, new[]
			{
				new Vector4(0.0f, 0.5f, 0.5f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
				new Vector4(0.5f, -0.5f, 0.5f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
				new Vector4(-0.5f, -0.5f, 0.5f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
			});

			// Prepare all the stages
			_deviceContext.InputAssembler.InputLayout = layout;
			_deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
			_deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 32, 0));
			_deviceContext.VertexShader.Shader = vertexShader;
			_deviceContext.Rasterizer.SetViewports(new Viewport(0, 0, demoConfiguration.Width, demoConfiguration.Height, 0.0f, 1.0f));
			_deviceContext.PixelShader.Shader = pixelShader;
			_deviceContext.OutputMerger.SetTargets(depthStencilView, _renderTargetView);
		}

		protected override void Draw(DemoTime time)
		{
			_deviceContext.ClearRenderTargetView(_renderTargetView, new Color4());
			_deviceContext.Draw(3, 0);
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
			program.Run(new DemoConfiguration("Rasterizr - Basic Triangle Sample") { WaitVerticalBlanking = true });
		}
	}
}
