using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Nexus;
using Rasterizr.Core.ShaderCore.GeometryShader;
using Rasterizr.Core.ShaderCore.VertexShader;

namespace Rasterizr.Tests.Core.ShaderCore.GeometryShader
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
			var geometryShaderInputs = new List<IVertexShaderOutput>
			{
				new TestVertexColor { Position = new Point4D(1, 0, 0, 1), Color = ColorsF.Red },
				new TestVertexColor { Position = new Point4D(0, 1, 0, 1), Color = ColorsF.Red },
				new TestVertexColor { Position = new Point4D(0, 0, 1, 1), Color = ColorsF.Red }
			};

			// Act.
			var result = geometryShaderStage.Run(geometryShaderInputs).ToList();

			// Assert.
			Assert.That(result, Has.Count.EqualTo(3));
			Assert.That(result, Has.All.InstanceOf<TestVertexColor>());
		}
	}
}