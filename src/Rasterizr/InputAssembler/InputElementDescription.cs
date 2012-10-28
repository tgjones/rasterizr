namespace Rasterizr.InputAssembler
{
	public class InputElementDescription
	{
		public string SemanticName { get; set; }
		public int SemanticIndex { get; set; }

		public InputElementDescription(string semanticName, int semanticIndex)
		{
			SemanticName = semanticName;
			SemanticIndex = semanticIndex;
		}
	}
}