using Rasterizr.VertexAttributes;

namespace Rasterizr.PipelineStages.TriangleSetup
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