namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.TriangleSetup
{
	public struct VertexGradients<TVertexShaderOutputVertexGradients>
	{
		public float OneOverZ;
		public TVertexShaderOutputVertexGradients AdditionalGradients;
	}
}