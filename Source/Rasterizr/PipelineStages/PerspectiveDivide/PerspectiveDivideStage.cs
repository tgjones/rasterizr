using System.Collections.Generic;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader;
using Nexus;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.PerspectiveDivide
{
	public class PerspectiveDivideStage : PipelineStageBase<VertexShaderOutput, ScreenSpaceVertex>
	{
		public int ScreenWidth { get; set; }
		public int ScreenHeight { get; set; }

		public PerspectiveDivideStage()
		{
			// TODO: Remove this
			ScreenWidth = 500;
			ScreenHeight = 300;
		}

		public override void Process(IList<VertexShaderOutput> inputs, IList<ScreenSpaceVertex> outputs)
		{
			int screenWidthDiv2 = ScreenWidth / 2;
			int screenHeightDiv2 = ScreenHeight / 2;
			int offsetX = 0;
			int offsetY = 0;

			Matrix3D viewportScaleMatrix = Matrix3D.CreateScale(screenWidthDiv2, -screenHeightDiv2, 0.5f)
				* Matrix3D.CreateTranslation(screenWidthDiv2 + offsetX, screenHeightDiv2 + offsetY, 0.5f);
			foreach (VertexShaderOutput vertexShaderOutput in inputs)
			{
				// Transform to viewport.
				Point4D viewportPosition = viewportScaleMatrix.Transform(vertexShaderOutput.Position);

				// Do perspective division.
				Point3D screenPosition = viewportPosition.ToPoint3D();

				outputs.Add(new ScreenSpaceVertex
				{
					Position = screenPosition,
					W = vertexShaderOutput.Position.W,
					Attributes = vertexShaderOutput.Attributes
				});
			}
		}
	}
}