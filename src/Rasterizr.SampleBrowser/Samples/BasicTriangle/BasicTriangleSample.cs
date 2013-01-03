using System.ComponentModel.Composition;
using System.Windows.Controls;
using Rasterizr.Math;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.Rasterizer;
using Rasterizr.Platform.Wpf;
using Rasterizr.Resources;
using SlimShader.Compiler;

namespace Rasterizr.SampleBrowser.Samples.BasicTriangle
{
	[Export(typeof(SampleBase))]
	[ExportMetadata("SortOrder", 1)]
	public class BasicTriangleSample : SampleBase
	{
		private DeviceContext _deviceContext;
		private RenderTargetView _renderTargetView;
		private WpfSwapChain _swapChain;

		public override string Name
		{
			get { return "Basic Triangle"; }
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
			var backBuffer = Texture2D.FromSwapChain<Texture2D>(_swapChain, 0);
			_renderTargetView = device.CreateRenderTargetView(backBuffer);

			// Compile Vertex and Pixel shaders
			var vertexShaderByteCode = ShaderCompiler.CompileFromFile("Samples/BasicTriangle/MiniTri.fx", "VS", "vs_4_0");
			var vertexShader = device.CreateVertexShader(vertexShaderByteCode);

			var pixelShaderByteCode = ShaderCompiler.CompileFromFile("Samples/BasicTriangle/MiniTri.fx", "PS", "ps_4_0");
			var pixelShader = device.CreatePixelShader(pixelShaderByteCode);

			// Layout from VertexShader input signature
			var layout = device.CreateInputLayout(
				new[]
				{
					new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0),
					new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 0)
				}, vertexShaderByteCode);

			// Instantiate Vertex buffer from vertex data
			var vertices = device.CreateBuffer(new BufferDescription(BindFlags.VertexBuffer), new[]
			{
				new Vector4(0.0f, 0.5f, 0.5f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
				new Vector4(0.5f, -0.5f, 0.5f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
				new Vector4(-0.5f, -0.5f, 0.5f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
			});

			// Prepare all the stages
			_deviceContext.InputAssembler.InputLayout = layout;
			_deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
			_deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 0, 32));
			_deviceContext.VertexShader.Shader = vertexShader;
			_deviceContext.Rasterizer.SetViewports(new Viewport(0, 0, width, height, 0.0f, 1.0f));
			_deviceContext.PixelShader.Shader = pixelShader;
			_deviceContext.OutputMerger.SetTargets(null, _renderTargetView);
		}

		public override void Draw(DemoTime time)
		{
			_deviceContext.ClearRenderTargetView(_renderTargetView, new Color4F(0, 0, 0, 1));
			_deviceContext.Draw(3, 0);
			_swapChain.Present();
		}
	}
}
