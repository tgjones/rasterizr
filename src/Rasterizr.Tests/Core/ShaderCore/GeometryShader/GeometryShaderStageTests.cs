using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Nexus;
using Nexus.Graphics.Colors;
using Rasterizr.Core.ShaderCore;
using Rasterizr.Core.ShaderCore.GeometryShader;

namespace Rasterizr.Tests.Core.ShaderCore.GeometryShader
{
	[TestFixture]
	public class GeometryShaderStageTests
	{
		private struct TestVertexColor
		{
			public Point4D Position;
			public ColorF Color;
		}

		[Test]
		public void CanUseGeometryShader()
		{
			// Arrange.
			var geometryShaderStage = new GeometryShaderStage();
			var geometryShaderInputs = new List<TransformedVertex>
			{
				new TransformedVertex(new TestVertexColor { Position = new Point4D(1, 0, 0, 1), Color = ColorsF.Red }, new Point4D(1, 0, 0, 1)),
				new TransformedVertex(new TestVertexColor { Position = new Point4D(0, 1, 0, 1), Color = ColorsF.Red }, new Point4D(0, 1, 0, 1)),
				new TransformedVertex(new TestVertexColor { Position = new Point4D(0, 0, 1, 1), Color = ColorsF.Red }, new Point4D(0, 0, 1, 1))
			};

			// Act.
			var result = geometryShaderStage.Run(geometryShaderInputs).ToList();

			// Assert.
			Assert.That(result, Has.Count.EqualTo(3));
			Assert.That(result, Has.All.Matches<TransformedVertex>(tv => tv.ShaderOutput is TestVertexColor));
		}
	}
}