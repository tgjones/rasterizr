using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using Rasterizr.Pipeline;
using Rasterizr.Pipeline.GeometryShader;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.PixelShader;
using Rasterizr.Pipeline.VertexShader;
using Rasterizr.Platform.Wpf;
using Rasterizr.Resources;
using Rasterizr.Toolkit.Models;
using SharpDX;
using SlimShader.Compiler;
using Buffer = Rasterizr.Resources.Buffer;
using Viewport = Rasterizr.Pipeline.Rasterizer.Viewport;

namespace Rasterizr.Studio.Modules.SampleBrowser.Samples.EnvironmentMapping
{
	[Export(typeof(SampleBase))]
	[ExportMetadata("SortOrder", 5)]
	public class EnvironmentMappingSample : SampleBase
	{
        private const int Width = 600;
        private const int Height = 400;

		private DeviceContext _deviceContext;
		private RenderTargetView _renderTargetView;
		private DepthStencilView _depthView;
		private SwapChain _swapChain;

	    private const int CubeMapSize = 128;
	    private RenderTargetView _renderTargetViewCube;
	    private DepthStencilView _depthViewCube;
	    private ShaderResourceView _resourceViewCube;

        private VertexShader _vertexShaderCubeMap, _vertexShaderStandard;
	    private GeometryShader _geometryShaderCubeMap;
	    private PixelShader _pixelShaderCubeMap, _pixelShaderStandard, _pixelShaderReflective;
        private Buffer _vertexConstantBuffer;
	    private VertexShaderData _vertexShaderData;
        private Buffer _geometryConstantBuffer;
	    private Matrix _view1, _view2, _view3, _view4, _view5, _view6;
	    private Matrix _projection, _projectionCube;
		private Model _model;
	    private Matrix _viewProjection, _viewProjectionCube;

		public override string Name
		{
			get { return "Environment Mapping"; }
		}

        public override WriteableBitmap Initialize(Device device)
		{
			// Create device and swap chain.
            var swapChainPresenter = new WpfSwapChainPresenter();
            _swapChain = device.CreateSwapChain(Width, Height, swapChainPresenter);
			_deviceContext = device.ImmediateContext;

			// Create RenderTargetView from the backbuffer.
			var backBuffer = Texture2D.FromSwapChain(_swapChain, 0);
			_renderTargetView = device.CreateRenderTargetView(backBuffer);

			// Create the depth buffer
			var depthBuffer = device.CreateTexture2D(new Texture2DDescription
			{
				ArraySize = 1,
				MipLevels = 1,
				Width = Width,
				Height = Height,
				BindFlags = BindFlags.DepthStencil
			});

			// Create the depth buffer view
			_depthView = device.CreateDepthStencilView(depthBuffer);

            // Create cube map resources.
            var cubeMapTexture = device.CreateTexture2D(new Texture2DDescription
            {
                Width = CubeMapSize,
                Height= CubeMapSize,
                ArraySize = 6,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                MipLevels = 1
            });
            _renderTargetViewCube = device.CreateRenderTargetView(cubeMapTexture);
            _resourceViewCube = device.CreateShaderResourceView(cubeMapTexture);
            var depthBufferCube = device.CreateTexture2D(new Texture2DDescription
            {
                ArraySize = 6,
                MipLevels = 1,
                Width = CubeMapSize,
                Height = CubeMapSize,
                BindFlags = BindFlags.DepthStencil
            });
            _depthViewCube = device.CreateDepthStencilView(depthBufferCube);

            // Load model.
		    var modelLoader = new ModelLoader(device, TextureLoader.CreateTextureFromStream);
            _model = modelLoader.Load("Modules/SampleBrowser/Samples/EnvironmentMapping/teapot.obj");

            // Compile Vertex and Pixel shaders
            var vertexShaderByteCode = ShaderCompiler.CompileFromFile("Modules/SampleBrowser/Samples/EnvironmentMapping/EnvironmentMapping.fx", "VS_CubeMap", "vs_4_0");
            _vertexShaderCubeMap = device.CreateVertexShader(vertexShaderByteCode);
            _vertexShaderStandard = device.CreateVertexShader(ShaderCompiler.CompileFromFile("Modules/SampleBrowser/Samples/EnvironmentMapping/EnvironmentMapping.fx", "VS_Standard", "vs_4_0"));

            _geometryShaderCubeMap = device.CreateGeometryShader(ShaderCompiler.CompileFromFile("Modules/SampleBrowser/Samples/EnvironmentMapping/EnvironmentMapping.fx", "GS_CubeMap", "gs_4_0"));

            _pixelShaderCubeMap = device.CreatePixelShader(ShaderCompiler.CompileFromFile("Modules/SampleBrowser/Samples/EnvironmentMapping/EnvironmentMapping.fx", "PS_CubeMap", "ps_4_0"));
            _pixelShaderStandard = device.CreatePixelShader(ShaderCompiler.CompileFromFile("Modules/SampleBrowser/Samples/EnvironmentMapping/EnvironmentMapping.fx", "PS_Standard", "ps_4_0"));
            _pixelShaderReflective = device.CreatePixelShader(ShaderCompiler.CompileFromFile("Modules/SampleBrowser/Samples/EnvironmentMapping/EnvironmentMapping.fx", "PS_Reflective", "ps_4_0"));

		    _model.SetInputLayout(device, vertexShaderByteCode.InputSignature);

            var sampler = device.CreateSamplerState(SamplerStateDescription.Default);

            // Create constant buffers.
            _vertexConstantBuffer = device.CreateBuffer(new BufferDescription
            {
                SizeInBytes = Utilities.SizeOf<VertexShaderData>(),
                BindFlags = BindFlags.ConstantBuffer
            });
            _geometryConstantBuffer = device.CreateBuffer(new BufferDescription
            {
                SizeInBytes = Utilities.SizeOf<GeometryShaderData>(),
                BindFlags = BindFlags.ConstantBuffer
            });
            var pixelConstantBuffer = device.CreateBuffer(new BufferDescription
            {
                SizeInBytes = Utilities.SizeOf<PixelShaderData>(),
                BindFlags = BindFlags.ConstantBuffer
            });

			// Prepare all the stages
            _deviceContext.VertexShader.SetConstantBuffers(0, _vertexConstantBuffer);
            _deviceContext.GeometryShader.SetConstantBuffers(0, _geometryConstantBuffer);
            _deviceContext.PixelShader.SetConstantBuffers(0, pixelConstantBuffer);
			_deviceContext.PixelShader.SetSamplers(0, sampler);

            _vertexShaderData = new VertexShaderData();

            _view1 = Matrix.LookAtLH(Vector3.Zero, new Vector3(1, 0, 0), Vector3.UnitY);
            _view2 = Matrix.LookAtLH(Vector3.Zero, new Vector3(-1, 0, 0), Vector3.UnitY);
            _view3 = Matrix.LookAtLH(Vector3.Zero, new Vector3(0, 1, 0), -Vector3.UnitZ);
            _view4 = Matrix.LookAtLH(Vector3.Zero, new Vector3(0, -1, 0), Vector3.UnitZ);
            _view5 = Matrix.LookAtLH(Vector3.Zero, new Vector3(0, 0, 1), Vector3.UnitY);
            _view6 = Matrix.LookAtLH(Vector3.Zero, new Vector3(0, 0, -1), Vector3.UnitY);

            var from = new Vector3(0, 30, 70);
            var to = new Vector3(0, 0, 0);

            // Prepare matrices
            var view = Matrix.LookAtLH(from, to, Vector3.UnitY);
            
			_projectionCube = Matrix.PerspectiveFovLH(MathUtil.PiOverTwo, 1.0f, 1f, 10000.0f);
            _projection = Matrix.PerspectiveFovLH(MathUtil.Pi / 3.0f, Width / (float) Height, 1f, 10000.0f);

            _viewProjectionCube = Matrix.Multiply(view, _projectionCube);
            _viewProjection = Matrix.Multiply(view, _projection);

            var pixelShaderData = new PixelShaderData
            {
                LightDirection = new Vector4(Vector3.Normalize(new Vector3(0.5f, 0, -1)), 1),
                EyePosition = new Vector4(from, 1)
            };
            _deviceContext.SetBufferData(pixelConstantBuffer, ref pixelShaderData);

            // Setup targets and viewport for rendering
            _deviceContext.Rasterizer.SetViewports(new Viewport(0, 0, Width, Height, 0.0f, 1.0f));
            _deviceContext.OutputMerger.SetTargets(_depthView, _renderTargetView);

            return swapChainPresenter.Bitmap;
		}

        public override void Draw(float time)
        {
            var world = Matrix.Translation(0, 0, 50) * Matrix.RotationY(time);

            // Render cubemap.
            _deviceContext.Rasterizer.SetViewports(new Viewport(0, 0, CubeMapSize, CubeMapSize, 0.0f, 1.0f));
            _deviceContext.OutputMerger.SetTargets(_depthViewCube, _renderTargetViewCube);
            _deviceContext.ClearDepthStencilView(_depthViewCube, DepthStencilClearFlags.Depth, 1.0f, 0);
            _deviceContext.ClearRenderTargetView(_renderTargetViewCube, Color4.CornflowerBlue);
            _deviceContext.VertexShader.Shader = _vertexShaderCubeMap;
            _deviceContext.GeometryShader.Shader = _geometryShaderCubeMap;
            _deviceContext.PixelShader.Shader = _pixelShaderCubeMap;
            _deviceContext.PixelShader.SetShaderResources(1, null);
            _vertexShaderData.World = world;
            _vertexShaderData.WorldViewProjection = _vertexShaderData.World * _viewProjectionCube;
            _deviceContext.SetBufferData(_vertexConstantBuffer, ref _vertexShaderData);

            var geometryShaderData = new GeometryShaderData
            {
                Matrix1 = world * _view1 * _projectionCube,
                Matrix2 = world * _view2 * _projectionCube,
                Matrix3 = world * _view3 * _projectionCube,
                Matrix4 = world * _view4 * _projectionCube,
                Matrix5 = world * _view5 * _projectionCube,
                Matrix6 = world * _view6 * _projectionCube
            };
            _deviceContext.SetBufferData(_geometryConstantBuffer, ref geometryShaderData);
            _model.Draw(_deviceContext);

            // Render scene.
            _deviceContext.Rasterizer.SetViewports(new Viewport(0, 0, Width, Height, 0.0f, 1.0f));
            _deviceContext.OutputMerger.SetTargets(_depthView, _renderTargetView);
            _deviceContext.ClearDepthStencilView(_depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
            _deviceContext.ClearRenderTargetView(_renderTargetView, Color4.Blue);

            // Draw rotating teapot.
            _deviceContext.VertexShader.Shader = _vertexShaderStandard;
            _deviceContext.GeometryShader.Shader = null;
            _deviceContext.PixelShader.Shader = _pixelShaderStandard;
            _vertexShaderData.World = world;
            _vertexShaderData.WorldViewProjection = _vertexShaderData.World * _viewProjection;
            _deviceContext.SetBufferData(_vertexConstantBuffer, ref _vertexShaderData);
		    _model.Draw(_deviceContext);

            // Draw stationary teapot.
            _deviceContext.VertexShader.Shader = _vertexShaderStandard;
            _deviceContext.GeometryShader.Shader = null;
            _deviceContext.PixelShader.Shader = _pixelShaderReflective;
            _vertexShaderData.World = Matrix.Identity;
            _vertexShaderData.WorldViewProjection = _vertexShaderData.World * _viewProjection;
            _deviceContext.SetBufferData(_vertexConstantBuffer, ref _vertexShaderData);
            _deviceContext.PixelShader.SetShaderResources(1, _resourceViewCube);
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
        private struct GeometryShaderData
        {
            public Matrix Matrix1;
            public Matrix Matrix2;
            public Matrix Matrix3;
            public Matrix Matrix4;
            public Matrix Matrix5;
            public Matrix Matrix6;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PixelShaderData
        {
            public Vector4 LightDirection;
            public Vector4 EyePosition;
        };
	}
}
