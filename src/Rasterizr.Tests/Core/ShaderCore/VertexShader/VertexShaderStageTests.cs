using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Nexus;
using Rasterizr.Core.ShaderCore.VertexShader;

namespace Rasterizr.Tests.Core.ShaderCore.VertexShader
{
	[TestFixture]
	public class VertexShaderStageTests
	{
		private struct TestVertexColor : IVertexShaderOutput
		{
			public Point4D Position { get; set; }
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
			var vertexShaderStage = new VertexShaderStage { VertexShader = new TestVertexShader() };
			var vertexShaderInputs = new List<object>
			{
				new TestVertexPositionColor(new Point3D(1, 0, 0), ColorsF.Red), 
				new TestVertexPositionColor(new Point3D(0, 1, 0), ColorsF.Red),
				new TestVertexPositionColor(new Point3D(0, 0, 1), ColorsF.Red)
			};

			// Act.
			var result = vertexShaderStage.Run(vertexShaderInputs).ToList();

			// Assert.
			Assert.That(result, Has.Count.EqualTo(3));
			Assert.That(result, Has.All.InstanceOf<TestVertexColor>());
		}
	}
}