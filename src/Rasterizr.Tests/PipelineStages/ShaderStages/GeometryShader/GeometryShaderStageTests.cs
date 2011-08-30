using System.Collections.Concurrent;
using NUnit.Framework;
using Nexus;
using Rasterizr.ShaderStages.GeometryShader;
using Rasterizr.ShaderStages.VertexShader;

namespace Rasterizr.Tests.PipelineStages.ShaderStages.GeometryShader
{
	[TestFixture]
	public class GeometryShaderStageTests
	{
		private struct TestVertexColor : IVertexShaderOutput
		{
			public Point4D Position { get; set; }
			public ColorF Color;
		}

		[Test]
		public void CanUseGeometryShader()
		{
			// Arrange.
			var geometryShaderStage = new GeometryShaderStage();

			var geometryShaderInputs = new BlockingCollection<IVertexShaderOutput>
			{
				new TestVertexColor { Position = new Point4D(1, 0, 0, 1), Color = ColorsF.Red },
				new TestVertexColor { Position = new Point4D(0, 1, 0, 1), Color = ColorsF.Red },
				new TestVertexColor { Position = new Point4D(0, 0, 1, 1), Color = ColorsF.Red }
			};
			geometryShaderInputs.CompleteAdding();

			var geometryShaderOutputs = new BlockingCollection<IVertexShaderOutput>();

			// Act.
			geometryShaderStage.Run(geometryShaderInputs, geometryShaderOutputs);

			// Assert.
			Assert.That(geometryShaderOutputs.IsAddingCompleted, Is.True);
			Assert.That(geometryShaderOutputs, Has.Count.EqualTo(3));
			Assert.That(geometryShaderOutputs, Has.All.InstanceOf<TestVertexColor>());
		}
	}
}