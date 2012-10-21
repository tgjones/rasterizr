using SlimShader.IO;

namespace SlimShader.ObjectModel
{
	public class OutputSignatureChunk : DxbcChunk
	{
		public static OutputSignatureChunk Parse(BytecodeReader reader)
		{
			return new OutputSignatureChunk();
		}
	}
}