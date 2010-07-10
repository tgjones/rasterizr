namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader.VertexAttributes
{
	public struct VertexAttribute
	{
		public string Name;
		public VertexAttributeInterpolationType InterpolationType;
		public IVertexAttributeValue Value;
	}
}