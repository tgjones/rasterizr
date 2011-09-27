﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Nexus;
using Nexus.Graphics;
using Rasterizr.Core.OutputMerger;
using Rasterizr.Core.Rasterizer;
using Rasterizr.Core.ShaderCore;
using Rasterizr.Core.ShaderCore.PixelShader;
using Rasterizr.Core.ShaderCore.VertexShader;

namespace Rasterizr.Tests.Core.Rasterizer
{
	[TestFixture]
	public class RasterizerStageTests
	{
		private struct TestVertexColor
		{
			public Point4D Position;
			public ColorF Color;
		}

		private class TestVertexShader : VertexShaderBase<TestVertexColor, TestVertexColor>
		{
			public override TestVertexColor Execute(TestVertexColor input)
			{
				return input;
			}
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
			var vertexShaderStage = new VertexShaderStage
			{
				VertexShader = new TestVertexShader()
			};
			var pixelShaderStage = new PixelShaderStage
			{
				PixelShader = new TestPixelShader()
			};
			var outputMergerStage = new OutputMergerStage
			{
				RenderTarget = new RenderTargetView(new ColorSurface(400, 300, 1))
			};
			var rasterizerStage = new RasterizerStage(vertexShaderStage, pixelShaderStage, outputMergerStage)
			{
				Viewport = new Viewport3D { Width = 400, Height = 300, MinDepth = 0, MaxDepth = 1 }
			};
			var rasterizerInputs = new List<TransformedVertex>
			{
				new TransformedVertex(new TestVertexColor { Position = new Point4D(-1, -1, 0, 1), Color = ColorsF.Red }, new Point4D(-1, -1, 0, 1)),
				new TransformedVertex(new TestVertexColor { Position = new Point4D(1, -1, 0, 1), Color = ColorsF.Red }, new Point4D(1, -1, 0, 1)),
				new TransformedVertex(new TestVertexColor { Position = new Point4D(-1, 1, 0, 1), Color = ColorsF.Red }, new Point4D(-1, 1, 0, 1))
			};

			// Act.
			var result = rasterizerStage.Run(rasterizerInputs).ToList();

			// Assert.
			Assert.That(result, Has.Count.EqualTo(60000));
			foreach (var fragment in result)
			{
				Assert.That(fragment.PixelShaderInput, Is.Not.Null);
				Assert.That(fragment.Samples, Is.Not.Null);
			}
		}
	}
}