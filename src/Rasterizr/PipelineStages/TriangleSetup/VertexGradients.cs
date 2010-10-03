namespace Rasterizr.PipelineStages.TriangleSetup
{
	public struct VertexGradients<TVertexShaderOutputVertexGradients>
	{
		public float OneOverZ;
		public TVertexShaderOutputVertexGradients AdditionalGradients;
	}
}