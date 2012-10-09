using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Nexus;
using Rasterizr.Core;
using Rasterizr.Core.InputAssembler;
using Rasterizr.Core.ShaderCore;
using Rasterizr.Core.ShaderCore.VertexShader;

namespace Rasterizr.Tests.Core.ShaderCore.VertexShader
{
	[TestFixture]
	public class VertexShaderStageTests
	{
		private struct TestVertexColor
		{
			[Semantic(SystemValueType.Position)]
			public Point4D Position;

			public ColorF Color;
		}

		private class TestVertexShader : VertexShaderBase<TestVertexPositionColor, TestVertexColor>
		{
			public override TestVertexColor Execute(TestVertexPositionColor vertexShaderInput)
			{
				return new TestVertexColor
				{
					Position = vertexShaderInput.Position.ToHomogeneousPoint3D(),
					Color = vertexShaderInput.Color
				};
			}
		}

		[Test]
		public void CanUseVertexShader()
		{
			// Arrange.
			var inputAssemblerStage = new InputAssemblerStage(Substitute.For<RasterizrDevice>())
			{
				InputLayout = new InputLayout(TestVertexPositionColor.InputElements, new TestVertexShader()),
			};
			var vertexShaderStage = new VertexShaderStage(inputAssemblerStage) { VertexShader = new TestVertexShader() };
			var vertexShaderInputs = new List<InputAssemblerOutput>
			{
				new InputAssemblerOutput(0, 0, new TestVertexPositionColor(new Point3D(1, 0, 0), ColorsF.Red)), 
				new InputAssemblerOutput(1, 0, new TestVertexPositionColor(new Point3D(0, 1, 0), ColorsF.Red)),
				new InputAssemblerOutput(2, 0, new TestVertexPositionColor(new Point3D(0, 0, 1), ColorsF.Red))
			};

			// Act.
			var result = vertexShaderStage.Run(vertexShaderInputs).ToList();

			// Assert.
			Assert.That(result, Has.Count.EqualTo(3));
			Assert.That(result, Has.All.Matches<TransformedVertex>(tv => tv.ShaderOutput is TestVertexColor));
		}
	}
}