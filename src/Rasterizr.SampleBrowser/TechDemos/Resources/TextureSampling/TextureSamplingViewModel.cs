using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Media.Imaging;
using Nexus;
using Rasterizr.Math;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.Rasterizer;
using Rasterizr.Platform.Wpf;
using Rasterizr.Resources;
using Rasterizr.SampleBrowser.Framework.Services;
using Rasterizr.Util;
using SlimShader.Compiler;

namespace Rasterizr.SampleBrowser.TechDemos.Resources.TextureSampling
{
	[Export(typeof(TechDemoViewModel))]
	[ExportMetadata("SortOrder", 1)]
	public class TextureSamplingViewModel : TechDemoViewModel
	{
		private readonly IResourceLoader _resourceLoader;

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
			_resourceLoader = resourceLoader;
		}

		protected override void OnActivate()
		{
			GenerateImage();
			base.OnActivate();
		}

		private void GenerateImage()
		{
			const int width = 400;
			const int height = 400;

			// Create device and swap chain.
			var device = new Device();
			var swapChain = new WpfSwapChain(device, width, height);
			Bitmap = swapChain.Bitmap;
			var deviceContext = device.ImmediateContext;

			// Create RenderTargetView from the backbuffer.
			var backBuffer = Texture2D.FromSwapChain(swapChain, 0);
			var renderTargetView = device.CreateRenderTargetView(backBuffer);

			// Create the depth buffer
			var depthBuffer = device.CreateTexture2D(new Texture2DDescription
			{
				ArraySize = 1,
				MipLevels = 1,
				Width = Bitmap.PixelWidth,
				Height = Bitmap.PixelHeight,
				BindFlags = BindFlags.DepthStencil
			});

			// Create the depth buffer view
			var depthView = device.CreateDepthStencilView(depthBuffer);

			// Compile Vertex and Pixel shaders
			var vertexShaderByteCode = ShaderCompiler.CompileFromFile("TechDemos/Resources/TextureSampling/Texture.fx", "VS", "vs_4_0");
			var vertexShader = device.CreateVertexShader(vertexShaderByteCode);

			var pixelShaderByteCode = ShaderCompiler.CompileFromFile("TechDemos/Resources/TextureSampling/Texture.fx", "PS", "ps_4_0");
			var pixelShader = device.CreatePixelShader(pixelShaderByteCode);

			// Layout from VertexShader input signature
			var layout = device.CreateInputLayout(
				new[]
				{
					new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
					new InputElement("TEXCOORD", 0, Format.R32G32_Float, 0, 16)
				}, vertexShaderByteCode);

			// Instantiate Vertex buiffer from vertex data
			var vertices = device.CreateBuffer(new BufferDescription(BindFlags.VertexBuffer), new[]
			{
				// 3D coordinates              UV Texture coordinates
                -1.0f, -1.0f, -1.0f, 1.0f,     0.0f, 1.0f, // Front
                -1.0f,  1.0f, -1.0f, 1.0f,     0.0f, 0.0f,
                1.0f,  1.0f, -1.0f, 1.0f,     1.0f, 0.0f,
                -1.0f, -1.0f, -1.0f, 1.0f,     0.0f, 1.0f,
                1.0f,  1.0f, -1.0f, 1.0f,     1.0f, 0.0f,
                1.0f, -1.0f, -1.0f, 1.0f,     1.0f, 1.0f,

                -1.0f, -1.0f,  1.0f, 1.0f,     1.0f, 0.0f, // BACK
                1.0f,  1.0f,  1.0f, 1.0f,     0.0f, 1.0f,
                -1.0f,  1.0f,  1.0f, 1.0f,     1.0f, 1.0f,
                -1.0f, -1.0f,  1.0f, 1.0f,     1.0f, 0.0f,
                1.0f, -1.0f,  1.0f, 1.0f,     0.0f, 0.0f,
                1.0f,  1.0f,  1.0f, 1.0f,     0.0f, 1.0f,

                -1.0f, 1.0f, -1.0f,  1.0f,     0.0f, 1.0f, // Top
                -1.0f, 1.0f,  1.0f,  1.0f,     0.0f, 0.0f,
                1.0f, 1.0f,  1.0f,  1.0f,     1.0f, 0.0f,
                -1.0f, 1.0f, -1.0f,  1.0f,     0.0f, 1.0f,
                1.0f, 1.0f,  1.0f,  1.0f,     1.0f, 0.0f,
                1.0f, 1.0f, -1.0f,  1.0f,     1.0f, 1.0f,

                -1.0f,-1.0f, -1.0f,  1.0f,     1.0f, 0.0f, // Bottom
                1.0f,-1.0f,  1.0f,  1.0f,     0.0f, 1.0f,
                -1.0f,-1.0f,  1.0f,  1.0f,     1.0f, 1.0f,
                -1.0f,-1.0f, -1.0f,  1.0f,     1.0f, 0.0f,
                1.0f,-1.0f, -1.0f,  1.0f,     0.0f, 0.0f,
                1.0f,-1.0f,  1.0f,  1.0f,     0.0f, 1.0f,

                -1.0f, -1.0f, -1.0f, 1.0f,     0.0f, 1.0f, // Left
                -1.0f, -1.0f,  1.0f, 1.0f,     0.0f, 0.0f,
                -1.0f,  1.0f,  1.0f, 1.0f,     1.0f, 0.0f,
                -1.0f, -1.0f, -1.0f, 1.0f,     0.0f, 1.0f,
                -1.0f,  1.0f,  1.0f, 1.0f,     1.0f, 0.0f,
                -1.0f,  1.0f, -1.0f, 1.0f,     1.0f, 1.0f,

                1.0f, -1.0f, -1.0f, 1.0f,     1.0f, 0.0f, // Right
                1.0f,  1.0f,  1.0f, 1.0f,     0.0f, 1.0f,
                1.0f, -1.0f,  1.0f, 1.0f,     1.0f, 1.0f,
                1.0f, -1.0f, -1.0f, 1.0f,     1.0f, 0.0f,
                1.0f,  1.0f, -1.0f, 1.0f,     0.0f, 0.0f,
                1.0f,  1.0f,  1.0f, 1.0f,     0.0f, 1.0f
			});

			// Create Constant Buffer
			var constantBuffer = device.CreateBuffer(new BufferDescription
			{
				SizeInBytes = Utilities.SizeOf<Matrix3D>(),
				BindFlags = BindFlags.ConstantBuffer
			});

			// Load texture and create sampler
			var textureStream = _resourceLoader.OpenResource("TechDemos/Resources/TextureSampling/CorrodedTiles.png");
			var texture = TextureLoader.CreateTextureFromFile(device, textureStream);
			var textureView = device.CreateShaderResourceView(texture);

			var sampler = device.CreateSamplerState(new SamplerStateDescription
			{
				Filter = SelectedTextureFilter,
				AddressU = TextureAddressMode.Wrap,
				AddressV = TextureAddressMode.Wrap,
				AddressW = TextureAddressMode.Wrap,
				BorderColor = Color4F.Black,
				ComparisonFunction = Comparison.Never,
				MaximumAnisotropy = 16,
				MipLodBias = 0,
				MinimumLod = 0,
				MaximumLod = 16,
			});

			// Prepare all the stages
			deviceContext.InputAssembler.InputLayout = layout;
			deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
			deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 0, Utilities.SizeOf<Vector4>() + Utilities.SizeOf<Vector2>()));
			deviceContext.VertexShader.SetConstantBuffers(0, constantBuffer);
			deviceContext.VertexShader.Shader = vertexShader;
			deviceContext.PixelShader.Shader = pixelShader;
			deviceContext.PixelShader.SetSamplers(0, sampler);
			deviceContext.PixelShader.SetShaderResources(0, textureView);

			// Setup targets and viewport for rendering
			deviceContext.Rasterizer.SetViewports(new Viewport(0, 0, Bitmap.PixelWidth, Bitmap.PixelHeight, 0.0f, 1.0f));
			deviceContext.OutputMerger.SetTargets(depthView, renderTargetView);

			// Prepare matrices
			var view = Matrix3D.CreateLookAt(new Point3D(0.7f, 0, -1.7f), Vector3D.Backward, Vector3D.UnitY);
			var projection = Matrix3D.CreatePerspectiveFieldOfView(Nexus.MathUtility.PI / 4.0f,
				Bitmap.PixelWidth / (float) Bitmap.PixelHeight, 0.1f, 100.0f);

			// Clear views
			deviceContext.ClearDepthStencilView(depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
			deviceContext.ClearRenderTargetView(renderTargetView, new Color4F(0.3f, 0, 0, 1));

			// Update WorldViewProj Matrix
			var worldViewProj = Matrix3D.CreateRotationX(0.3f)
				* Matrix3D.CreateRotationY(0.6f)
				* Matrix3D.CreateRotationZ(0.6f)
				* view * projection;
			worldViewProj = Matrix3D.Transpose(worldViewProj);
			constantBuffer.SetData(ref worldViewProj);

			// Draw the cube
			deviceContext.Draw(36, 0);

			// Present!
			swapChain.Present();
		}
	}
}