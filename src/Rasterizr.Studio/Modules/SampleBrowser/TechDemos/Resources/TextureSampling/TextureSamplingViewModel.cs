using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Media.Imaging;
using Gemini.Framework.Services;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Platform.Wpf;
using Rasterizr.Resources;
using SharpDX;
using SlimShader.Compiler;
using Buffer = Rasterizr.Resources.Buffer;
using Utilities = Rasterizr.Util.Utilities;
using Viewport = Rasterizr.Pipeline.Rasterizer.Viewport;

namespace Rasterizr.Studio.Modules.SampleBrowser.TechDemos.Resources.TextureSampling
{
	[Export(typeof(TechDemoViewModel))]
	[ExportMetadata("SortOrder", 1)]
	public class TextureSamplingViewModel : TechDemoViewModel
	{
		private readonly ImageGenerator _imageGenerator;

		public override string Category
		{
			get { return "Resources"; }
		}

		private WriteableBitmap _bitmap;
		public WriteableBitmap Bitmap
		{
			get { return _bitmap; }
			set
			{
				_bitmap = value;
				NotifyOfPropertyChange(() => Bitmap);
			}
		}

		public IEnumerable<Filter> TextureFilters
		{
			get { return Enum.GetValues(typeof(Filter)).Cast<Filter>(); }
		}

		private Filter _selectedTextureFilter = Filter.MinMagMipPoint;
	    public Filter SelectedTextureFilter
		{
			get { return _selectedTextureFilter; }
			set
			{
				_selectedTextureFilter = value;
				NotifyOfPropertyChange(() => SelectedTextureFilter);
				GenerateImage();
			}
		}

		[ImportingConstructor]
		public TextureSamplingViewModel(IResourceManager resourceLoader)
		{
			_imageGenerator = new ImageGenerator(resourceLoader);

		    DisplayName = "Texture Sampling";
		}

		protected override void OnActivate()
		{
			GenerateImage();
			base.OnActivate();
		}

	    private void GenerateImage()
	    {
            _imageGenerator.GenerateImage(SelectedTextureFilter);
	        Bitmap = _imageGenerator.OutputBitmap;
	    }

	    private class ImageGenerator
	    {
	        private readonly WriteableBitmap _outputBitmap;
            private readonly Device _device;
            private readonly DeviceContext _deviceContext;
	        private readonly Matrix _view, _projection;
            private readonly RenderTargetView _renderTargetView;
            private readonly DepthStencilView _depthView;
	        private readonly SwapChain _swapChain;
	        private readonly Buffer _constantBuffer;

	        public WriteableBitmap OutputBitmap
	        {
	            get { return _outputBitmap; }
	        }

            public ImageGenerator(IResourceManager resourceLoader)
	        {
                const int width = 400;
                const int height = 400;

                // Create device and swap chain.
                _device = new Device();
                var swapChainPresenter = new WpfSwapChainPresenter();
                _swapChain = _device.CreateSwapChain(width, height, swapChainPresenter);
                _outputBitmap = swapChainPresenter.Bitmap;
                _deviceContext = _device.ImmediateContext;

                // Create RenderTargetView from the backbuffer.
                var backBuffer = Texture2D.FromSwapChain(_swapChain, 0);
                _renderTargetView = _device.CreateRenderTargetView(backBuffer);

                // Create the depth buffer
                var depthBuffer = _device.CreateTexture2D(new Texture2DDescription
                {
                    ArraySize = 1,
                    MipLevels = 1,
                    Width = width,
                    Height = height,
                    BindFlags = BindFlags.DepthStencil
                });

                // Create the depth buffer view
                _depthView = _device.CreateDepthStencilView(depthBuffer);

                // Compile Vertex and Pixel shaders
                var vertexShaderByteCode = ShaderCompiler.CompileFromFile("Modules/SampleBrowser/TechDemos/Resources/TextureSampling/Texture.fx", "VS", "vs_4_0");
                var vertexShader = _device.CreateVertexShader(vertexShaderByteCode);

                var pixelShaderByteCode = ShaderCompiler.CompileFromFile("Modules/SampleBrowser/TechDemos/Resources/TextureSampling/Texture.fx", "PS", "ps_4_1");
                var pixelShader = _device.CreatePixelShader(pixelShaderByteCode);

                // Layout from VertexShader input signature
                var layout = _device.CreateInputLayout(
                    new[]
	                {
	                    new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
	                    new InputElement("TEXCOORD", 0, Format.R32G32_Float, 0, 16)
	                }, vertexShaderByteCode);

                // Instantiate Vertex buiffer from vertex data
                var vertices = _device.CreateBuffer(new BufferDescription(BindFlags.VertexBuffer), new[]
	            {
	                // 3D coordinates              UV Texture coordinates
	                -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 1.0f, // Front
	                -1.0f, 1.0f, -1.0f, 1.0f, 0.0f, 0.0f,
	                1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 0.0f,
	                -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 1.0f,
	                1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 0.0f,
	                1.0f, -1.0f, -1.0f, 1.0f, 1.0f, 1.0f,

	                -1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 0.0f, // BACK
	                1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 1.0f,
	                -1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f,
	                -1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
	                1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f,
	                1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 1.0f,

	                -1.0f, 1.0f, -1.0f, 1.0f, 0.0f, 1.0f, // Top
	                -1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f,
	                1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
	                -1.0f, 1.0f, -1.0f, 1.0f, 0.0f, 1.0f,
	                1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
	                1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f,

	                -1.0f, -1.0f, -1.0f, 1.0f, 1.0f, 0.0f, // Bottom
	                1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 1.0f,
	                -1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 1.0f,
	                -1.0f, -1.0f, -1.0f, 1.0f, 1.0f, 0.0f,
	                1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 0.0f,
	                1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 1.0f,

	                -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 1.0f, // Left
	                -1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f,
	                -1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
	                -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 1.0f,
	                -1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
	                -1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f,

	                1.0f, -1.0f, -1.0f, 1.0f, 1.0f, 0.0f, // Right
	                1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 1.0f,
	                1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 1.0f,
	                1.0f, -1.0f, -1.0f, 1.0f, 1.0f, 0.0f,
	                1.0f, 1.0f, -1.0f, 1.0f, 0.0f, 0.0f,
	                1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 1.0f
	            });

                // Create Constant Buffer
                _constantBuffer = _device.CreateBuffer(new BufferDescription
                {
                    SizeInBytes = Utilities.SizeOf<Matrix>(),
                    BindFlags = BindFlags.ConstantBuffer
                });

                // Load texture and create sampler
                var textureStream = resourceLoader.GetStream(
                    "Modules/SampleBrowser/TechDemos/Resources/TextureSampling/CorrodedTiles.png",
                    GetType().Assembly.FullName);
                var texture = TextureLoader.CreateTextureFromStream(_device, textureStream);
                var textureView = _device.CreateShaderResourceView(texture);

                // Prepare all the stages
                _deviceContext.InputAssembler.InputLayout = layout;
                _deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
                _deviceContext.InputAssembler.SetVertexBuffers(0,
                    new VertexBufferBinding(vertices, 0, Utilities.SizeOf<Vector4>() + Utilities.SizeOf<Vector2>()));
                _deviceContext.VertexShader.SetConstantBuffers(0, _constantBuffer);
                _deviceContext.VertexShader.Shader = vertexShader;
                _deviceContext.PixelShader.Shader = pixelShader;
                _deviceContext.PixelShader.SetShaderResources(0, textureView);

                // Setup targets and viewport for rendering
                _deviceContext.Rasterizer.SetViewports(new Viewport(0, 0, _outputBitmap.PixelWidth, _outputBitmap.PixelHeight, 0.0f, 1.0f));
                _deviceContext.OutputMerger.SetTargets(_depthView, _renderTargetView);

                // Prepare matrices
                _view = Matrix.LookAtRH(new Vector3(0.7f, 0, -1.7f), Vector3.BackwardRH, Vector3.UnitY);
                _projection = Matrix.PerspectiveFovRH(MathUtil.PiOverFour,
                    _outputBitmap.PixelWidth / (float) _outputBitmap.PixelHeight, 0.1f, 100.0f);
	        }

	        public void GenerateImage(Filter textureFilter)
	        {
                var sampler = _device.CreateSamplerState(new SamplerStateDescription
                {
                    Filter = textureFilter,
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
                _deviceContext.PixelShader.SetSamplers(0, sampler);

	            // Clear views
	            _deviceContext.ClearDepthStencilView(_depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
                _deviceContext.ClearRenderTargetView(_renderTargetView, new Color4(0.3f, 0, 0, 1));

	            // Update WorldViewProj Matrix
	            var worldViewProj = Matrix.RotationX(0.3f)
	                * Matrix.RotationY(0.6f)
	                * Matrix.RotationZ(0.6f)
	                * _view * _projection;
	            worldViewProj = Matrix.Transpose(worldViewProj);
                _deviceContext.SetBufferData(_constantBuffer, ref worldViewProj);

	            // Draw the cube
	            _deviceContext.Draw(36, 0);

	            // Present!
                _deviceContext.Present(_swapChain);
	        }
	    }
	}
}