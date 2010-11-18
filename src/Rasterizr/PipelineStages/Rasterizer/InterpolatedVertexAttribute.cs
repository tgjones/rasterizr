using Rasterizr.VertexAttributes;

namespace Rasterizr.PipelineStages.Rasterizer
{
	public struct InterpolatedVertexAttribute
	{
		public string Name;
		public string Semantic;
		public IVertexAttributeValue Value;

		// Used by pixel shader for texture mipmapping.
		/// <summary>
		/// Change in value per horizontal pixel step
		/// </summary>
		public IVertexAttributeValue DValueDx;

		/// <summary>
		/// Change in value per vertical pixel step.
		/// </summary>
		public IVertexAttributeValue DValueDy;
	}
}