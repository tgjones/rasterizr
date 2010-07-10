using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader.VertexAttributes;
using Nexus;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.PerspectiveDivide
{
	public struct ScreenSpaceVertex
	{
		public Point3D Position;
		public float W;
		public VertexAttribute[] Attributes;
	}
}