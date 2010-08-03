using Rasterizr.Util;

namespace Rasterizr.PipelineStages.TriangleSetup
{
	public class InterpolatedVertexAttributeCollection : PartiallyActiveCollection<InterpolatedVertexAttribute>
	{
		public InterpolatedVertexAttributeCollection(int capacity)
			: base(capacity)
		{
			
		}
	}
}