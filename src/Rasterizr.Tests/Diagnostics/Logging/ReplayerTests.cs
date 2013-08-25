using System.IO;
using System.Linq;
using NUnit.Framework;
using Rasterizr.Diagnostics.Logging;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Resources;
using SharpDX;
using SlimShader;
using SlimShader.Compiler;
using Viewport = Rasterizr.Pipeline.Rasterizer.Viewport;

namespace Rasterizr.Tests.Diagnostics.Logging
{
	[TestFixture]
	public class ReplayerTests
	{
		[Test]
		public void EndToEndTest()
		{	
			// Arrange.
		    var device = new Device();
            var logger = new TracefileBuilder(device);
            var expectedData = RenderScene(device);

            var stringWriter = new StringWriter();
			logger.WriteTo(stringWriter);

			var loggedJson = stringWriter.ToString();
			var logReader = new StringReader(loggedJson);
			var tracefile = Tracefile.FromTextReader(logReader);

			// Act.
			var swapChainPresenter = new RawSwapChainPresenter();
			var replayer = new Replayer(
                tracefile.Frames[0], tracefile.Frames[0].Events.Last(),
                swapChainPresenter);
			replayer.Replay();
            var actualData = swapChainPresenter.Data;

			// Assert.
			Assert.That(actualData, Is.EqualTo(expectedData));
		}

        private static Number4[] RenderScene(Device device)
		{
			const int width = 200;
			const int height = 100;

			// Create device and swap chain.
            var swapChainPresenter = new RawSwapChainPresenter();
			var swapChain = device.CreateSwapChain(
                new SwapChainDescription(width, height), 
                swapChainPresenter);
			var deviceContext = device.ImmediateContext;

			// Create RenderTargetView from the backbuffer.
			var backBuffer = Texture2D.FromSwapChain(swapChain, 0);
			var renderTargetView = device.CreateRenderTargetView(backBuffer);

			// Compile Vertex and Pixel shaders
			var vertexShaderByteCode = ShaderCompiler.CompileFromFile("Assets/MiniTri.fx", "VS", "vs_4_0");
			var vertexShader = device.CreateVertexShader(vertexShaderByteCode);

			var pixelShaderByteCode = ShaderCompiler.CompileFromFile("Assets/MiniTri.fx", "PS", "ps_4_0");
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
			deviceContext.InputAssembler.InputLayout = layout;
			deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
			deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 0, 32));
			deviceContext.VertexShader.Shader = vertexShader;
			deviceContext.Rasterizer.SetViewports(new Viewport(0, 0, width, height, 0.0f, 1.0f));
			deviceContext.PixelShader.Shader = pixelShader;
			deviceContext.OutputMerger.SetTargets(null, renderTargetView);

            deviceContext.ClearRenderTargetView(renderTargetView, Color4.Black);
			deviceContext.Draw(3, 0);
			swapChain.Present();

            return swapChainPresenter.Data;
		}
	}
}