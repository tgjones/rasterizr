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
            _model = modelLoader.Load("Modules/SampleBrowser/Samples/ModelLoading/Sponza/sponza.3ds");

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
                LightPos = new Vector4(0, 2.5f, 0, 0)
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
			_projection = Matrix.PerspectiveFovLH(MathUtil.PiOverFour, 
				width / (float) height, 0.1f, 100.0f);

            return swapChainPresenter.Bitmap;
		}

        public override void Draw(float time)
		{
			// Clear views
			_deviceContext.ClearDepthStencilView(_depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
            _deviceContext.ClearRenderTargetView(_renderTargetView, Color4.Black);

            // Rotate camera
            //var cameraPosition = new Vector3(0, 3, 5.0f);
            Vector3 cameraPosition = new Vector3(0, 5, 3.0f);
            var cameraLookAt = new Vector3(0, 2.0f, 0);
            //Vector4 tempPos = Vector3.Transform(cameraPosition, Matrix.RotationY(0.2f * time.ElapsedTime));
            //cameraPosition = new Vector3(tempPos.X, tempPos.Y, tempPos.Z);

            // Calculate the view matrix.
            var view = Matrix.LookAtLH(cameraPosition, cameraLookAt, Vector3.UnitY);
            var viewProjection = Matrix.Multiply(view, _projection);

            // Update transformation matrices.
            _vertexShaderData.World = Matrix.Translation(0, -_model.AxisAlignedBoxCentre.Y / 2, 0);
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
            public Vector4 LightPos;
        };
	}
}
