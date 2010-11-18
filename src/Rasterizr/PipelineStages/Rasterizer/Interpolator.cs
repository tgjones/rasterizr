using Nexus;
using Rasterizr.VertexAttributes;

namespace Rasterizr.PipelineStages.Rasterizer
{
	public struct Interpolator
	{
		public IVertexAttributeValue Edge1;
		public IVertexAttributeValue Edge2;
		public IVertexAttributeValue Edge3;

		public IVertexAttributeValue GetInterpolatedValue(Point2D samplePosition)
		{
			return Edge1.Multiply(samplePosition.X).Add(Edge2.Multiply(samplePosition.Y)).Add(Edge3);
		}
	}

	public struct Interpolator<TVertexAttributeValue>
		where TVertexAttributeValue : IVertexAttributeValue
	{
		public IVertexAttributeValue Edge1;
		public IVertexAttributeValue Edge2;
		public IVertexAttributeValue Edge3;

		public TVertexAttributeValue GetInterpolatedValue(Point2D samplePosition)
		{
			return (TVertexAttributeValue) Edge1.Multiply(samplePosition.X).Add(Edge2.Multiply(samplePosition.Y)).Add(Edge3);
		}
	}
}