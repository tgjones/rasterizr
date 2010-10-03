namespace Rasterizr.VertexAttributes
{
	public struct VertexAttribute
	{
		public string Name;
		public string Semantic;
		public VertexAttributeInterpolationType InterpolationType;
		public IVertexAttributeValue Value;
	}
}