using System.IO;
using NUnit.Framework;
using Rasterizr.Diagnostics.Logging;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Math;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.PixelShader;
using Rasterizr.Pipeline.Rasterizer;
using Rasterizr.Pipeline.VertexShader;
using Rasterizr.Resources;
using SlimShader.Compiler;

namespace Rasterizr.Tests.Diagnostics.Logging
{
	[TestFixture]
	public class ReplayerTests
	{
		[Test]
		public void EndToEndTest()
		{
			// Arrange.
			var stringWriter = new StringWriter();
			var logger = new TracefileGraphicsLogger(stringWriter);
			var expectedData = RenderScene(logger);
			logger.Flush();
			var loggedJson = stringWriter.ToString();
			var logReader = new StringReader(loggedJson);
			var tracefile = Tracefile.FromTextReader(logReader);

			// Act.
			RawSwapChain swapChain = null;
			var replayer = new Replayer(tracefile.Frames[0], (d, desc) =>
			{
				swapChain = new RawSwapChain(d, desc.Width, desc.Height);
				return swapChain;
			});
			replayer.Replay();
			var actualData = swapChain.Data;

			// Assert.
			Assert.That(actualData, Is.EqualTo(expectedData));
		}

		private static byte[] RenderScene(TracefileGraphicsLogger logger)
		{
			int width = 200;
			int height = 100;

			// Create device and swap chain.
			var device = new Device(logger);
			var swapChain = new RawSwapChain(device, width, height);
			var deviceContext = device.ImmediateContext;

			// Create RenderTargetView from the backbuffer.
			var backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
			var renderTargetView = new RenderTargetView(device, backBuffer);

			// Compile Vertex and Pixel shaders
			var vertexShaderByteCode = ShaderCompiler.CompileFromFile("Assets/MiniTri.fx", "VS", "vs_4_0");
			var vertexShader = new VertexShader(device, vertexShaderByteCode);

			var pixelShaderByteCode = ShaderCompiler.CompileFromFile("Assets/MiniTri.fx", "PS", "ps_4_0");
			var pixelShader = new PixelShader(device, pixelShaderByteCode);

			// Layout from VertexShader input signature
			var layout = new InputLayout(device,
				vertexShaderByteCode.InputSignature,
				new[]
				{
					new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0),
					new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 0)
				});

			// Instantiate Vertex buffer from vertex data
			var vertices = Buffer.Create(device, BindFlags.VertexBuffer, new[]
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

			deviceContext.ClearRenderTargetView(renderTargetView, new Color4F(0, 0, 0, 1));
			deviceContext.Draw(3, 0);
			swapChain.Present();

			return swapChain.Data;
		}
	}
}