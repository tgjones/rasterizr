using Nexus;
using Rasterizr.PipelineStages.Rasterizer;

namespace Rasterizr.VertexAttributes
{
	public interface IVertexAttributeValue
	{
		object Value { get; }

		IVertexAttributeValue Add(IVertexAttributeValue value);
		IVertexAttributeValue Multiply(float f);
		IVertexAttributeValue Subtract(IVertexAttributeValue value);
	}
}