using Rasterizr.VertexAttributes;

namespace Rasterizr.PipelineStages.InputAssembler
{
	public class InputElementDescription
	{
		public string Name { get; set; }
		public VertexAttributeValueFormat Format { get; set; }
		public InputElementUsage Usage { get; set; }

		public InputElementDescription(string name, VertexAttributeValueFormat format,
			InputElementUsage usage)
		{
			Name = name;
			Format = format;
			Usage = usage;
		}
	}
}