using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader.VertexAttributes;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.TriangleSetup
{
	public struct VertexAttributeStepValue
	{
		public IVertexAttributeValue Value;
		public IVertexAttributeValue ValueStep;

		public void Step()
		{
			Value = Value.Add(ValueStep);
		}
	}
}