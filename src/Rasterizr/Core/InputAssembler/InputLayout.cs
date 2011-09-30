using System.Linq;
using Rasterizr.Core.ShaderCore;

namespace Rasterizr.Core.InputAssembler
{
	public class InputLayout
	{
		public InputElementDescription[] Elements { get; private set; }

		public InputLayout(InputElementDescription[] elements, IShader vertexShader)
		{
			Elements = elements;

			// Validate that elements match the vertex shader inputs.
			new ShaderValidator().CheckCompatibility(this, vertexShader);
		}

		public bool ContainsSemantic(string name, int index)
		{
			return Elements.Any(e => e.SemanticName == name && e.SemanticIndex == index);
		}

		public InputElementDescription GetElementBySemantic(string name, int index)
		{
			return Elements.First(e => e.SemanticName == name && e.SemanticIndex == index);
		}
	}
}