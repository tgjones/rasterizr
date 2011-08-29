using System.Collections.Concurrent;
using NUnit.Framework;
using Nexus;
using Rasterizr.ShaderStages.VertexShader;

namespace Rasterizr.Tests.PipelineStages.ShaderStages.VertexShader
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

			var vertexShaderInputs = new BlockingCollection<object>
			{
				new TestVertexPositionColor(new Point3D(1, 0, 0), ColorsF.Red), 
				new TestVertexPositionColor(new Point3D(0, 1, 0), ColorsF.Red),
				new TestVertexPositionColor(new Point3D(0, 0, 1), ColorsF.Red)
			};
			vertexShaderInputs.CompleteAdding();

			var vertexShaderOutputs = new BlockingCollection<IVertexShaderOutput>();

			// Act.
			vertexShaderStage.Run(vertexShaderInputs, vertexShaderOutputs);

			// Assert.
			Assert.That(vertexShaderOutputs.IsAddingCompleted, Is.True);
			Assert.That(vertexShaderOutputs, Has.Count.EqualTo(3));
			Assert.That(vertexShaderOutputs, Has.All.InstanceOf<TestVertexColor>());
		}
	}
}