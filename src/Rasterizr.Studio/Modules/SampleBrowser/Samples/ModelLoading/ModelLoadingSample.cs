using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Platform.Wpf;
using Rasterizr.Resources;
using Rasterizr.Toolkit.Models;
using SharpDX;
using SlimShader.Compiler;
using Buffer = Rasterizr.Resources.Buffer;
using Viewport = Rasterizr.Pipeline.Rasterizer.Viewport;

namespace Rasterizr.Studio.Modules.SampleBrowser.Samples.ModelLoading
{
	[Export(typeof(SampleBase))]
	[ExportMetadata("SortOrder", 4)]
	public class ModelLoadingSample : SampleBase
	{
		private DeviceContext _deviceContext;
		private RenderTargetView _renderTargetView;
		private DepthStencilView _depthView;
		private SwapChain _swapChain;

        private Buffer _vertexConstantBuffer;
        private Buffer _pixelConstantBuffer;
	    private VertexShaderData _vertexShaderData;
	    private PixelShaderData _pixelShaderData;
		private Model _model;
	    private Matrix _projection;

		public override string Name
		{
			get { return "Model Loading"; }
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

			// Create the depth buffer
			var depthBuffer = device.CreateTexture2D(new Texture2DDescription
			{
				ArraySize = 1,
				MipLevels = 1,
				Width = width,
				Height = height,
				BindFlags = BindFlags.DepthStencil
			});

			// Create the depth buffer view
			_depthView = device.CreateDepthStencilView(depthBuffer);

            // Load model.
		    var modelLoader = new ModelLoader(device, TextureLoader.CreateTextureFromStream);
            _model = modelLoader.Load("Modules/SampleBrowser/Samples/ModelLoading/Car/car.obj");

            // Compile Vertex and Pixel shaders
            var vertexShaderByteCode = ShaderCompiler.CompileFromFile("Modules/SampleBrowser/Samples/ModelLoading/ModelLoading.fx", "VS", "vs_4_0");
            var vertexShader = device.CreateVertexShader(vertexShaderByteCode);

            var pixelShaderByteCode = ShaderCompiler.CompileFromFile("Modules/SampleBrowser/Samples/ModelLoading/ModelLoading.fx", "PS", "ps_4_0");
            var pixelShader = device.CreatePixelShader(pixelShaderByteCode);

		    _model.SetInputLayout(device, vertexShaderByteCode.InputSignature);

			var sampler = device.CreateSamplerState(new SamplerStateDescription
			{
				Filter = Filter.MinMagMipPoint,
				AddressU = TextureAddressMode.Wrap,
				AddressV = TextureAddressMode.Wrap,
				AddressW = TextureAddressMode.Wrap,
				BorderColor = Color4.Black,
				ComparisonFunction = Comparison.Never,
				MaximumAnisotropy = 16,
				MipLodBias = 0,
				MinimumLod = 0,
				MaximumLod = 16,
			});

            // Create constant buffers.
            _vertexConstantBuffer = device.CreateBuffer(new BufferDescription
            {
                SizeInBytes = Utilities.SizeOf<VertexShaderData>(),
                BindFlags = BindFlags.ConstantBuffer
            });
            _pixelConstantBuffer = device.CreateBuffer(new BufferDescription
            {
                SizeInBytes = Utilities.SizeOf<PixelShaderData>(),
                BindFlags = BindFlags.ConstantBuffer
            });

			// Prepare all the stages
            _deviceContext.VertexShader.SetConstantBuffers(0, _vertexConstantBuffer);
            _deviceContext.VertexShader.Shader = vertexShader;
            _deviceContext.PixelShader.SetConstantBuffers(0, _pixelConstantBuffer);
            _deviceContext.PixelShader.Shader = pixelShader;
			_deviceContext.PixelShader.SetSamplers(0, sampler);

            _vertexShaderData = new VertexShaderData();

            _pixelShaderData = new PixelShaderData
            {
                LightDirection = new Vector4(Vector3.Normalize(new Vector3(0.5f, 0, -1)), 1)
            };
            _deviceContext.SetBufferData(_pixelConstantBuffer, ref _pixelShaderData);

		    // Setup targets and viewport for rendering
			_deviceContext.Rasterizer.SetViewports(new Viewport(0, 0, width, height, 0.0f, 1.0f));
			_deviceContext.OutputMerger.SetTargets(_depthView, _renderTargetView);

            //_deviceContext.Rasterizer.State = device.CreateRasterizerState(new Pipeline.Rasterizer.RasterizerStateDescription
            //{
            //    FillMode = Pipeline.Rasterizer.FillMode.Wireframe
            //});

			// Prepare matrices
			_projection = Matrix.PerspectiveFovLH(MathUtil.Pi / 3.0f, 
				width / (float) height, 1f, 1000.0f);

            return swapChainPresenter.Bitmap;
		}

        public override void Draw(float time)
		{
			// Clear views
			_deviceContext.ClearDepthStencilView(_depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
            _deviceContext.ClearRenderTargetView(_renderTargetView, Color4.CornflowerBlue);

            // Calculate the view matrix.
            var view = Matrix.LookAtLH(new Vector3(0, 10, -30), Vector3.Zero, Vector3.UnitY);
            var viewProjection = Matrix.Multiply(view, _projection);

            // Update transformation matrices.
            _vertexShaderData.World = Matrix.RotationY(time);
            _vertexShaderData.WorldViewProjection = _vertexShaderData.World * viewProjection;

            // Transpose matrices before sending them to the shader.
            _vertexShaderData.World.Transpose();
            _vertexShaderData.WorldViewProjection.Transpose();

            _deviceContext.SetBufferData(_vertexConstantBuffer, ref _vertexShaderData);

		    _model.Draw(_deviceContext);

			// Present!
            _deviceContext.Present(_swapChain);

			base.Draw(time);
		}

        [StructLayout(LayoutKind.Sequential)]
        private struct VertexShaderData
        {
            public Matrix WorldViewProjection;
            public Matrix World;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PixelShaderData
        {
            public Vector4 LightDirection;
        };
	}
}
