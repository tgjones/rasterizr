using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Nexus;
using Rasterizr.PipelineStages.InputAssembler;
using Rasterizr.PipelineStages.ShaderStages.Core;

namespace Rasterizr.Tests.PipelineStages.InputAssembler
{
	[TestFixture]
	public class InputAssemblerStageTests
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct TestVertexPositionColor
		{
			public Point3D Position;
			public ColorF Color;

			public TestVertexPositionColor(Point3D position, ColorF color)
			{
				Position = position;
				Color = color;
			}

			public static InputLayout InputLayout
			{
				get
				{
					return new InputLayout
					{
						Elements = new[]
						{
							new InputElementDescription(Semantics.Position, 0),
							new InputElementDescription(Semantics.Color, 0)
						}
					};
				}
			}
		}

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
			var result = new BlockingCollection<object>();

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
			var result = new BlockingCollection<object>();

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
			var result = new BlockingCollection<object>();

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
			var result = new BlockingCollection<object>();

			// Act.
			inputAssemblerStage.Run(result);

			// Assert.
			Assert.That(result, Has.Count.EqualTo(6));
			Assert.That(result.IsAddingCompleted, Is.True);
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