using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Nexus;
using Rasterizr.InputAssembler;

namespace Rasterizr.Tests.PipelineStages.InputAssembler
{
	[TestFixture]
	public class InputAssemblerStageTests
	{
		[Test]
		public void CanUseInputAssemblerWithTriangleListWithoutIndices()
		{
			// Arrange.
			var inputAssemblerStage = new InputAssemblerStage
			{
				InputLayout = TestVertexPositionColor.InputLayout,
				PrimitiveTopology = PrimitiveTopology.TriangleList,
				Vertices = new[]
				{
					new TestVertexPositionColor(new Point3D(1, 0, 0), ColorsF.Red),
					new TestVertexPositionColor(new Point3D(0, 1, 0), ColorsF.Red),
					new TestVertexPositionColor(new Point3D(0, 0, 1), ColorsF.Red)
				}
			};
			var result = new List<object>();

			// Act.
			inputAssemblerStage.Run(result);

			// Assert.
			Assert.That(result, Has.Count.EqualTo(3));
		}

		[Test]
		public void CanUseInputAssemblerWithTriangleListAndIndices()
		{
			// Arrange.
			var inputAssemblerStage = new InputAssemblerStage
			{
				InputLayout = TestVertexPositionColor.InputLayout,
				PrimitiveTopology = PrimitiveTopology.TriangleList,
				Vertices = new[]
				{
					new TestVertexPositionColor(new Point3D(1, 0, 0), ColorsF.Red),
					new TestVertexPositionColor(new Point3D(0, 1, 0), ColorsF.Red),
					new TestVertexPositionColor(new Point3D(0, 0, 1), ColorsF.Red)
				},
				Indices = new Int32Collection(new[] { 0, 1, 2 })
			};
			var result = new List<object>();

			// Act.
			inputAssemblerStage.Run(result);

			// Assert.
			Assert.That(result, Has.Count.EqualTo(3));
		}

		[Test]
		public void CanUseInputAssemblerWithTriangleStripWithoutIndices()
		{
			// Arrange.
			var inputAssemblerStage = new InputAssemblerStage
			{
				InputLayout = TestVertexPositionColor.InputLayout,
				PrimitiveTopology = PrimitiveTopology.TriangleStrip,
				Vertices = new[]
				{
					new TestVertexPositionColor(new Point3D(1, 0, 0), ColorsF.Red),
					new TestVertexPositionColor(new Point3D(0, 1, 0), ColorsF.Red),
					new TestVertexPositionColor(new Point3D(0, 0, 1), ColorsF.Red),
					new TestVertexPositionColor(new Point3D(1, 1, 1), ColorsF.Red)
				}
			};
			var result = new List<object>();

			// Act.
			inputAssemblerStage.Run(result);

			// Assert.
			Assert.That(result, Has.Count.EqualTo(6));
		}

		[Test]
		public void CanUseInputAssemblerWithTriangleStripAndIndices()
		{
			// Arrange.
			var inputAssemblerStage = new InputAssemblerStage
			{
				InputLayout = TestVertexPositionColor.InputLayout,
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
			var result = new List<object>();

			// Act.
			inputAssemblerStage.Run(result);

			// Assert.
			Assert.That(result, Has.Count.EqualTo(6));
			Assert.That(result, Has.All.InstanceOf<TestVertexPositionColor>());
			var resultArray = result.Cast<TestVertexPositionColor>().ToArray();
			Assert.That(resultArray[0].Position, Is.EqualTo(new Point3D(1, 0, 0)));
			Assert.That(resultArray[1].Position, Is.EqualTo(new Point3D(0, 1, 0)));
			Assert.That(resultArray[2].Position, Is.EqualTo(new Point3D(0, 0, 1)));
			Assert.That(resultArray[3].Position, Is.EqualTo(new Point3D(0, 1, 0)));
			Assert.That(resultArray[4].Position, Is.EqualTo(new Point3D(1, 1, 1)));
			Assert.That(resultArray[5].Position, Is.EqualTo(new Point3D(0, 0, 1)));
		}
	}
}