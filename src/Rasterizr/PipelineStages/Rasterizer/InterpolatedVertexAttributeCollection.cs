using System.Linq;
using Rasterizr.PipelineStages.Rasterizer;
using Rasterizr.Util;

namespace Rasterizr.PipelineStages.TriangleSetup
{
	public class InterpolatedVertexAttributeCollection : PartiallyActiveCollection<InterpolatedVertexAttribute>
	{
		public InterpolatedVertexAttributeCollection(int capacity)
			: base(capacity)
		{
			
		}

		public InterpolatedVertexAttribute GetBySemantic(string semantic)
		{
			return this.Single(va => va.Semantic == semantic);
		}
	}
}