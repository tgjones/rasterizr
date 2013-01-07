using System.ComponentModel.Composition;
using System.Windows.Controls;
using Nexus;
using Rasterizr.Math;
using Rasterizr.Pipeline;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.Rasterizer;
using Rasterizr.Platform.Wpf;
using Rasterizr.Resources;
using Rasterizr.Util;
using SlimShader.Compiler;

namespace Rasterizr.SampleBrowser.Samples.MiniCubeTexture
{
	[Export(typeof(SampleBase))]
	[ExportMetadata("SortOrder", 3)]
	public class MiniCubeTextureSample : SampleBase
	{
		private DeviceContext _deviceContext;
		private RenderTargetView _renderTargetView;
		private DepthStencilView _depthView;
		private WpfSwapChain _swapChain;

		private Buffer _constantBuffer;
		private Matrix3D _view;
		private Matrix3D _projection;

		public override string Name
		{
			get { return "Rotating Cube (Textured)"; }
		}

		public override void Initialize(Image image)
		{
			const int width = 400;
			const int height = 300;

			// Create device and swap chain.
			var device = new Device();
			_swapChain = new WpfSwapChain(device, width, height);
			image.Source = _swapChain.Bitmap;
			_deviceContext = device.ImmediateContext;

			// Create RenderTargetView from the backbuffer.
			var backBuffer = Texture2D.FromSwapChain<Texture2D>(_swapChain, 0);
			_renderTargetView = device.CreateRenderTargetView(backBuffer);

			// Create the depth buffer
			var depthBuffer = device.CreateTexture2D(new Texture2DDescription
			{
				Format = Format.D32_Float_S8X24_UInt,
				ArraySize = 1,
				MipLevels = 1,
				Width = width,
				Height = height,
				BindFlags = BindFlags.DepthStencil
			});

			// Create the depth buffer view
			_depthView = device.CreateDepthStencilView(depthBuffer);

			// Compile Vertex and Pixel shaders
			var vertexShaderByteCode = ShaderCompiler.CompileFromFile("Samples/MiniCubeTexture/MiniCubeTexture.fx", "VS", "vs_4_0");
			var vertexShader = device.CreateVertexShader(vertexShaderByteCode);

			var pixelShaderByteCode = ShaderCompiler.CompileFromFile("Samples/MiniCubeTexture/MiniCubeTexture.fx", "PS", "ps_4_0");
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
			_constantBuffer = device.CreateBuffer(new BufferDescription
			{
				SizeInBytes = Utilities.SizeOf<Matrix3D>(),
				BindFlags = BindFlags.ConstantBuffer
			});

			// Load texture and create sampler
			var texture = device.CreateTexture2D(new Texture2DDescription());// Texture2D.FromFile<Texture2D>(device, "GeneticaMortarlessBlocks.jpg");
			var textureView = device.CreateShaderResourceView(texture);

			var sampler = new SamplerState(device, new SamplerStateDescription
			{
				Filter = Filter.MinMagMipLinear,
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
			_deviceContext.InputAssembler.InputLayout = layout;
			_deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
			_deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 0, Utilities.SizeOf<Vector4>() + Utilities.SizeOf<Vector2>()));
			_deviceContext.VertexShader.SetConstantBuffers(0, _constantBuffer);
			_deviceContext.VertexShader.Shader = vertexShader;
			_deviceContext.PixelShader.Shader = pixelShader;
			_deviceContext.PixelShader.SetSamplers(0, sampler);
			_deviceContext.PixelShader.SetShaderResources(0, textureView);

			// Setup targets and viewport for rendering
			_deviceContext.Rasterizer.SetViewports(new Viewport(0, 0, width, height, 0.0f, 1.0f));
			_deviceContext.OutputMerger.SetTargets(_depthView, _renderTargetView);

			// Prepare matrices
			_view = Matrix3D.CreateLookAt(new Point3D(0, 0, -5), Vector3D.Backward, Vector3D.UnitY);
			_projection = Matrix3D.CreatePerspectiveFieldOfView(MathUtility.PI / 4.0f, 
				width / (float) height, 0.1f, 100.0f);
		}

		public override void Draw(DemoTime time)
		{
			// Clear views
			_deviceContext.ClearDepthStencilView(_depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
			_deviceContext.ClearRenderTargetView(_renderTargetView, new Color4F(0, 0, 0, 1));

			// Update WorldViewProj Matrix
			var worldViewProj = Matrix3D.CreateRotationX((float)time.ElapsedTime)
				* Matrix3D.CreateRotationY((float)(time.ElapsedTime * 1))
				* Matrix3D.CreateRotationZ((float)(time.ElapsedTime * 0.3f))
				* _view * _projection;
			worldViewProj = Matrix3D.Transpose(worldViewProj);
			_constantBuffer.SetData(ref worldViewProj);

			// Draw the cube
			_deviceContext.Draw(36, 0);

			// Present!
			_swapChain.Present();

			base.Draw(time);
		}
	}
}
