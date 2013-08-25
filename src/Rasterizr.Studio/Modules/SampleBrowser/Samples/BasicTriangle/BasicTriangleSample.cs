using System.ComponentModel.Composition;
using System.Windows.Media.Imaging;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Platform.Wpf;
using Rasterizr.Resources;
using Rasterizr.Toolkit.Models;
using SharpDX;
using SlimShader.Compiler;
using Viewport = Rasterizr.Pipeline.Rasterizer.Viewport;

namespace Rasterizr.Studio.Modules.SampleBrowser.Samples.BasicTriangle
{
	[Export(typeof(SampleBase))]
	[ExportMetadata("SortOrder", 1)]
	public class BasicTriangleSample : SampleBase
	{
		private DeviceContext _deviceContext;
		private RenderTargetView _renderTargetView;
		private SwapChain _swapChain;
	    private Model _model;

		public override string Name
		{
			get { return "Basic Triangle"; }
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

			// Compile Vertex and Pixel shaders
			var vertexShaderByteCode = ShaderCompiler.CompileFromFile("Modules/SampleBrowser/Samples/BasicTriangle/MiniTri.fx", "VS", "vs_4_0");
			var vertexShader = device.CreateVertexShader(vertexShaderByteCode);

            var pixelShaderByteCode = ShaderCompiler.CompileFromFile("Modules/SampleBrowser/Samples/BasicTriangle/MiniTri.fx", "PS", "ps_4_0");
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

		    _model = new Model();
		    _model.AddMesh(new ModelMesh
		    {
		        IndexBuffer = device.CreateBuffer(new BufferDescription
		        {
		            BindFlags = BindFlags.IndexBuffer
		        }, new uint[] { 0, 1, 2 }),
                IndexCount = 3,
                InputLayout = layout,
                PrimitiveCount = 1,
                PrimitiveTopology = PrimitiveTopology.TriangleList,
                VertexBuffer = vertices,
                VertexCount = 3,
                VertexSize = 32
		    });

			// Prepare all the stages
			_deviceContext.VertexShader.Shader = vertexShader;
			_deviceContext.Rasterizer.SetViewports(new Viewport(0, 0, width, height, 0.0f, 1.0f));
			_deviceContext.PixelShader.Shader = pixelShader;
			_deviceContext.OutputMerger.SetTargets(null, _renderTargetView);

            return swapChainPresenter.Bitmap;
		}

        public override void Draw(float time)
		{
            _deviceContext.ClearRenderTargetView(_renderTargetView, Color4.Black);
		    _model.Draw(_deviceContext);
			_deviceContext.Present(_swapChain);
		}
	}
}
