using System.Linq;

namespace Rasterizr.PipelineStages.InputAssembler
{
	public class InputLayout
	{
		public InputElementDescription[] Elements { get; set; }

		public bool ContainsUsage(InputElementUsage usage)
		{
			return Elements.Any(e => e.Usage == usage);
		}
	}
}