using System.Collections.Generic;
using Nexus;
using Rasterizr.PipelineStages.PrimitiveAssembly;
using Rasterizr.PipelineStages.ShaderStages.Core;
using Rasterizr.PipelineStages.ShaderStages.VertexShader;
using Rasterizr.VertexAttributes;

namespace Rasterizr.PipelineStages.PerspectiveDivide
{
	public class PerspectiveDivideStage : PipelineStageBase<TrianglePrimitive, ScreenSpaceVertex>
	{
		public int ScreenWidth { get; set; }
		public int ScreenHeight { get; set; }

		public PerspectiveDivideStage(Viewport3D viewport)
		{
			// TODO: Remove this
			ScreenWidth = viewport.Width;
			ScreenHeight = viewport.Height;
		}

		public override void Process(IList<TrianglePrimitive> inputs, IList<ScreenSpaceVertex> outputs)
		{
			int screenWidthDiv2 = ScreenWidth / 2;
			int screenHeightDiv2 = ScreenHeight / 2;
			int offsetX = 0;
			int offsetY = 0;

			Matrix3D viewportScaleMatrix = Matrix3D.CreateScale(screenWidthDiv2, -screenHeightDiv2, 0.5f)
				* Matrix3D.CreateTranslation(screenWidthDiv2 + offsetX, screenHeightDiv2 + offsetY, 0.5f);

			// TODO: Pass through whole-primitive data.
			foreach (TrianglePrimitive primitive in inputs)
				foreach (VertexShaderOutput vertexShaderOutput in primitive.Vertices)
				{
					// Transform to viewport.
					Point4D viewportPosition = viewportScaleMatrix.Transform(vertexShaderOutput.Position);

					// Do perspective division.
					Point3D screenPosition = viewportPosition.ToPoint3D();

					// Add depth as a system-value attribute.
					// Add system-value semantics as requested by pixel shader (or by the system, such as SV_Depth).
					VertexAttributeCollection attributes = vertexShaderOutput.Attributes;
					attributes.Add(new VertexAttribute
					{
						Name = Semantics.SV_Depth,
						Semantic = Semantics.SV_Depth,
						InterpolationType = VertexAttributeInterpolationType.Perspective,
						Value = new FloatVertexAttributeValue { Value = screenPosition.Z }
					});

					outputs.Add(new ScreenSpaceVertex
					{
						Position = screenPosition,
						W = vertexShaderOutput.Position.W,
						Attributes = attributes
					});
				}
		}
	}
}