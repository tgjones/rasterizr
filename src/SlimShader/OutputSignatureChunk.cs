using SlimShader.IO;

namespace SlimShader
{
	public class OutputSignatureChunk : DxbcChunk
	{
		public static OutputSignatureChunk Parse(BytecodeReader reader)
		{
			return new OutputSignatureChunk();
		}
	}
}