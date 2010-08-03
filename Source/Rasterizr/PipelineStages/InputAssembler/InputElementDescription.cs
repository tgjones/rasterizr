using Rasterizr.VertexAttributes;

namespace Rasterizr.PipelineStages.InputAssembler
{
	public class InputElementDescription
	{
		public string Name { get; set; }
		public VertexAttributeValueFormat Format { get; set; }

		public InputElementDescription(string name, VertexAttributeValueFormat format)
		{
			Name = name;
			Format = format;
		}
	}
}