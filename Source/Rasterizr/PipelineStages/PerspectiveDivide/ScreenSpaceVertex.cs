using Nexus;
using Rasterizr.VertexAttributes;

namespace Rasterizr.PipelineStages.PerspectiveDivide
{
	public struct ScreenSpaceVertex
	{
		public Point3D Position;
		public float W;
		public VertexAttribute[] Attributes;
	}
}