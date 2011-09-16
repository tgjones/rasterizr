using System.Collections.Generic;
using NUnit.Framework;
using Nexus;
using Nexus.Graphics;
using Rasterizr.OutputMerger;
using Rasterizr.Rasterizer;
using Rasterizr.ShaderCore.PixelShader;
using Rasterizr.ShaderCore.VertexShader;

namespace Rasterizr.Tests.Rasterizer
{
	[TestFixture]
	public class RasterizerStageTests
	{
		private struct TestVertexColor : IVertexShaderOutput
		{
			public Point4D Position { get; set; }
			public ColorF Color;
		}

		private class TestPixelShader : PixelShaderBase<TestVertexColor>
		{
			public override ColorF Execute(TestVertexColor pixelShaderInput)
			{
				return pixelShaderInput.Color;
			}
		}

		[Test]
		public void CanRasterizerWithMultiSamplingDisabled()
		{
			// Arrange.
			var pixelShaderStage = new PixelShaderStage
			{
				PixelShader = new TestPixelShader()
			};
			var outputMergerStage = new OutputMergerStage
			{
				RenderTarget = new RenderTargetView(new ColorSurface(400, 300, 1))
			};
			var rasterizerStage = new RasterizerStage(pixelShaderStage, outputMergerStage)
			{
				Viewport = new Viewport3D { Width = 400, Height = 300, MinDepth = 0, MaxDepth = 1 }
			};

			var rasterizerInputs = new List<IVertexShaderOutput>
			{
				new TestVertexColor { Position = new Point4D(-1, -1, 0, 1), Color = ColorsF.Red },
				new TestVertexColor { Position = new Point4D(1, -1, 0, 1), Color = ColorsF.Red },
				new TestVertexColor { Position = new Point4D(-1, 1, 0, 1), Color = ColorsF.Red }
			};

			var rasterizerOutputs = new List<Fragment>();

			// Act.
			rasterizerStage.Run(rasterizerInputs, rasterizerOutputs);

			// Assert.
			Assert.That(rasterizerOutputs, Has.Count.EqualTo(60000));
			foreach (var fragment in rasterizerOutputs)
			{
				Assert.That(fragment.PixelShaderInput, Is.Not.Null);
				Assert.That(fragment.Samples, Is.Not.Null);
			}
		}
	}
}