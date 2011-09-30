using System.Linq;
using NUnit.Framework;
using Nexus;
using Rasterizr.Core.InputAssembler;
using Rasterizr.Core.ShaderCore;
using Rasterizr.Core.ShaderCore.VertexShader;

namespace Rasterizr.Tests.Core.InputAssembler
{
	[TestFixture]
	public class InputAssemblerStageTests
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
		public void CanUseInputAssemblerWithTriangleListWithoutIndices()
		{
			// Arrange.
			var inputAssemblerStage = new InputAssemblerStage
			{
				InputLayout = new InputLayout(TestVertexPositionColor.InputElements, new TestVertexShader()),
				PrimitiveTopology = PrimitiveTopology.TriangleList,
				Vertices = new[]
				{
					new TestVertexPositionColor(new Point3D(1, 0, 0), ColorsF.Red),
					new TestVertexPositionColor(new Point3D(0, 1, 0), ColorsF.Red),
					new TestVertexPositionColor(new Point3D(0, 0, 1), ColorsF.Red)
				}
			};

			// Act.
			var result = inputAssemblerStage.Run(3, 0).ToList();

			// Assert.
			Assert.That(result, Has.Count.EqualTo(3));
		}

		[Test]
		public void CanUseInputAssemblerWithTriangleListAndIndices()
		{
			// Arrange.
			var inputAssemblerStage = new InputAssemblerStage
			{
				InputLayout = new InputLayout(TestVertexPositionColor.InputElements, new TestVertexShader()),
				PrimitiveTopology = PrimitiveTopology.TriangleList,
				Vertices = new[]
				{
					new TestVertexPositionColor(new Point3D(1, 0, 0), ColorsF.Red),
					new TestVertexPositionColor(new Point3D(0, 1, 0), ColorsF.Red),
					new TestVertexPositionColor(new Point3D(0, 0, 1), ColorsF.Red)
				},
				Indices = new Int32Collection(new[] { 0, 1, 2 })
			};

			// Act.
			var result = inputAssemblerStage.RunIndexed(3, 0, 0).ToList();

			// Assert.
			Assert.That(result, Has.Count.EqualTo(3));
		}

		[Test]
		public void CanUseInputAssemblerWithTriangleStripWithoutIndices()
		{
			// Arrange.
			var inputAssemblerStage = new InputAssemblerStage
			{
				InputLayout = new InputLayout(TestVertexPositionColor.InputElements, new TestVertexShader()),
				PrimitiveTopology = PrimitiveTopology.TriangleStrip,
				Vertices = new[]
				{
					new TestVertexPositionColor(new Point3D(1, 0, 0), ColorsF.Red),
					new TestVertexPositionColor(new Point3D(0, 1, 0), ColorsF.Red),
					new TestVertexPositionColor(new Point3D(0, 0, 1), ColorsF.Red),
					new TestVertexPositionColor(new Point3D(1, 1, 1), ColorsF.Red)
				}
			};

			// Act.
			var result = inputAssemblerStage.Run(4, 0).ToList();

			// Assert.
			Assert.That(result, Has.Count.EqualTo(6));
		}

		[Test]
		public void CanUseInputAssemblerWithTriangleStripAndIndices()
		{
			// Arrange.
			var inputAssemblerStage = new InputAssemblerStage
			{
				InputLayout = new InputLayout(TestVertexPositionColor.InputElements, new TestVertexShader()),
				PrimitiveTopology = PrimitiveTopology.TriangleStrip,
				Vertices = new[]
				{
					new TestVertexPositionColor(new Point3D(1, 0, 0), ColorsF.Red),
					new TestVertexPositionColor(new Point3D(0, 1, 0), ColorsF.Red),
					new TestVertexPositionColor(new Point3D(0, 0, 1), ColorsF.Red),
					new TestVertexPositionColor(new Point3D(1, 1, 1), ColorsF.Red)
				},
				Indices = new Int32Collection(new[] { 0, 1, 2, 3 })
			};

			// Act.
			var result = inputAssemblerStage.RunIndexed(4, 0, 0).ToList();

			// Assert.
			Assert.That(result, Has.Count.EqualTo(6));
			Assert.That(result, Has.All.Matches<InputAssemblerOutput>(iao => iao.Vertex is TestVertexPositionColor));
			var resultArray = result.Select(iao => iao.Vertex).Cast<TestVertexPositionColor>().ToArray();
			Assert.That(resultArray[0].Position, Is.EqualTo(new Point3D(1, 0, 0)));
			Assert.That(resultArray[1].Position, Is.EqualTo(new Point3D(0, 1, 0)));
			Assert.That(resultArray[2].Position, Is.EqualTo(new Point3D(0, 0, 1)));
			Assert.That(resultArray[3].Position, Is.EqualTo(new Point3D(0, 1, 0)));
			Assert.That(resultArray[4].Position, Is.EqualTo(new Point3D(1, 1, 1)));
			Assert.That(resultArray[5].Position, Is.EqualTo(new Point3D(0, 0, 1)));
		}
	}
}