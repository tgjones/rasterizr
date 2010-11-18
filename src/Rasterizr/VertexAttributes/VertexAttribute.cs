namespace Rasterizr.VertexAttributes
{
	public struct VertexAttribute
	{
		public string Name;
		public string Semantic;
		public VertexAttributeInterpolationModifier InterpolationModifier;
		public IVertexAttributeValue Value;
	}
}