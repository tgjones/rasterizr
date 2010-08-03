using Rasterizr.VertexAttributes;

namespace Rasterizr.PipelineStages.TriangleSetup
{
	public struct VertexAttributeStepValue
	{
		public IVertexAttributeValue Value;
		public IVertexAttributeValue ValueStep;

		public void Step()
		{
			Value = Value.Add(ValueStep);
		}
	}
}