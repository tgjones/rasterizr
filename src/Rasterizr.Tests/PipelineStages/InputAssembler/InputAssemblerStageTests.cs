using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Nexus;
using Rasterizr.PipelineStages.InputAssembler;
using Rasterizr.PipelineStages.ShaderStages.VertexShader;
using Rasterizr.VertexAttributes;

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
						new InputElementDescription("Position", VertexAttributeValueFormat.Point3D, InputElementUsage.Position),
						new InputElementDescription("Color", VertexAttributeValueFormat.ColorF, InputElementUsage.Color)
					}
					};
				}
			}
		}

		[Test]
		public void CanUseInputAssemblerWithoutIndices()
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
			var result = new List<VertexShaderInput>();

			// Act.
			inputAssemblerStage.Process(result);
		}
	}
}