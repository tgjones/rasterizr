using System.Linq;

namespace Rasterizr.PipelineStages.InputAssembler
{
	public class InputLayout
	{
		public InputElementDescription[] Elements { get; set; }

		public bool ContainsSemantic(string name, int index)
		{
			return Elements.Any(e => e.SemanticName == name && e.SemanticIndex == index);
		}
	}
}