namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.GeometryShader
{
	public struct Triangle<TGeometryShaderInput>
	{
		public TGeometryShaderInput V1;
		public TGeometryShaderInput V2;
		public TGeometryShaderInput V3;
	}
}