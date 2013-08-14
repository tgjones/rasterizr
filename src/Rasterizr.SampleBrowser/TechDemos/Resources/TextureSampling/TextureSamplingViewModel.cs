using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Media.Imaging;
using Nexus;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.Rasterizer;
using Rasterizr.Platform.Wpf;
using Rasterizr.Resources;
using Rasterizr.SampleBrowser.Framework.Services;
using Rasterizr.Util;
using SlimShader;
using SlimShader.Compiler;
using Buffer = Rasterizr.Resources.Buffer;

namespace Rasterizr.SampleBrowser.TechDemos.Resources.TextureSampling
{
	[Export(typeof(TechDemoViewModel))]
	[ExportMetadata("SortOrder", 1)]
	public class TextureSamplingViewModel : TechDemoViewModel
	{
		private readonly ImageGenerator _imageGenerator;

		public override string DisplayName
		{
			get { return "Texture Sampling"; }
			set { base.DisplayName = value; }
		}

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
		public TextureSamplingViewModel(IResourceLoader resourceLoader)
		{
			_imageGenerator = new ImageGenerator(resourceLoader);
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
	        private readonly Matrix3D _view, _projection;
            private readonly RenderTargetView _renderTargetView;
            private readonly DepthStencilView _depthView;
	        private readonly WpfSwapChain _swapChain;
	        private readonly Buffer _constantBuffer;

	        public WriteableBitmap OutputBitmap
	        {
	            get { return _outputBitmap; }
	        }

	        public ImageGenerator(IResourceLoader resourceLoader)
	        {
                const int width = 400;
                const int height = 400;

                // Create device and swap chain.
                _device = new Device();
                _swapChain = new WpfSwapChain(_device, width, height);
                _outputBitmap = _swapChain.Bitmap;
                _deviceContext = _device.ImmediateContext;

                // Create RenderTargetView from the backbuffer.
                var backBuffer = Texture2D.FromSwapChain(_swapChain, 0);
                _renderTargetView = _device.CreateRenderTargetView(backBuffer);

                // Create the depth buffer
                var depthBuffer = _device.CreateTexture2D(new Texture2DDescription
                {
                    ArraySize = 1,
                    MipLevels = 1,
                    Width = _outputBitmap.PixelWidth,
                    Height = _outputBitmap.PixelHeight,
                    BindFlags = BindFlags.DepthStencil
                });

                // Create the depth buffer view
                _depthView = _device.CreateDepthStencilView(depthBuffer);

                // Compile Vertex and Pixel shaders
                var vertexShaderByteCode = ShaderCompiler.CompileFromFile("TechDemos/Resources/TextureSampling/Texture.fx", "VS", "vs_4_0");
                var vertexShader = _device.CreateVertexShader(vertexShaderByteCode);

                var pixelShaderByteCode = ShaderCompiler.CompileFromFile("TechDemos/Resources/TextureSampling/Texture.fx", "PS", "ps_4_1");
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
                    SizeInBytes = Utilities.SizeOf<Matrix3D>(),
                    BindFlags = BindFlags.ConstantBuffer
                });

                // Load texture and create sampler
                var textureStream = resourceLoader.OpenResource("TechDemos/Resources/TextureSampling/CorrodedTiles.png");
                var texture = TextureLoader.CreateTextureFromStream(_device, textureStream);
                var textureView = _device.CreateShaderResourceView(texture);

                // Prepare all the stages
                _deviceContext.InputAssembler.InputLayout = layout;
                _deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
                _deviceContext.InputAssembler.SetVertexBuffers(0,
                    new VertexBufferBinding(vertices, 0, Utilities.SizeOf<Vector4D>() + Utilities.SizeOf<Vector2D>()));
                _deviceContext.VertexShader.SetConstantBuffers(0, _constantBuffer);
                _deviceContext.VertexShader.Shader = vertexShader;
                _deviceContext.PixelShader.Shader = pixelShader;
                _deviceContext.PixelShader.SetShaderResources(0, textureView);

                // Setup targets and viewport for rendering
                _deviceContext.Rasterizer.SetViewports(new Viewport(0, 0, _outputBitmap.PixelWidth, _outputBitmap.PixelHeight, 0.0f, 1.0f));
                _deviceContext.OutputMerger.SetTargets(_depthView, _renderTargetView);

                // Prepare matrices
                _view = Matrix3D.CreateLookAt(new Point3D(0.7f, 0, -1.7f), Vector3D.Backward, Vector3D.UnitY);
                _projection = Matrix3D.CreatePerspectiveFieldOfView(Nexus.MathUtility.PI / 4.0f,
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
                    BorderColor = new Number4(0, 0, 0, 1),
                    ComparisonFunction = Comparison.Never,
                    MaximumAnisotropy = 16,
                    MipLodBias = 0,
                    MinimumLod = 0,
                    MaximumLod = 16,
                });
                _deviceContext.PixelShader.SetSamplers(0, sampler);

	            // Clear views
	            _deviceContext.ClearDepthStencilView(_depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
                _deviceContext.ClearRenderTargetView(_renderTargetView, new Number4(0.3f, 0, 0, 1));

	            // Update WorldViewProj Matrix
	            var worldViewProj = Matrix3D.CreateRotationX(0.3f)
	                * Matrix3D.CreateRotationY(0.6f)
	                * Matrix3D.CreateRotationZ(0.6f)
	                * _view * _projection;
	            worldViewProj = Matrix3D.Transpose(worldViewProj);
	            _constantBuffer.SetData(ref worldViewProj);

	            // Draw the cube
	            _deviceContext.Draw(36, 0);

	            // Present!
	            _swapChain.Present();
	        }
	    }
	}
}