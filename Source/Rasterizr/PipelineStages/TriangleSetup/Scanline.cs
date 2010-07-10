namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.TriangleSetup
{
	public struct Scanline
	{
		public int Y;

		public int XStart;
		public float XPrestep;

		public int Width;

		public InterpolatedVertexAttributes InterpolatedVertexAttributes;
	}
}