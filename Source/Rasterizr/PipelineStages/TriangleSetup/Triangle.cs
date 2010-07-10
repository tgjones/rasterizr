using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader.VertexAttributes;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.TriangleSetup
{
	public struct Triangle
	{
		public int YTop;
		public int YBottom;

		/// <summary>
		/// Scanlines sorted from YTop to YBottom
		/// </summary>
		public Scanline[] Scanlines;

		//public TriangleGradients Gradients;
		public IVertexAttributeValue[] VertexAttributeXStepValues;
		public IVertexAttributeValue[] VertexAttributeYStepValues;
		public float DOneOverWdX;
	}
}