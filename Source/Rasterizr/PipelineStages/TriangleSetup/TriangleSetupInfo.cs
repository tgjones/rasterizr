namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.TriangleSetup
{
	public struct TriangleSetupInfo
	{
		public TriangleGradients Gradients;
		public Edge TopToBottomEdge;
		public Edge TopToMiddleEdge;
		public Edge MiddleToBottomEdge;
	}
}