using SlimShader.IO;

namespace SlimShader
{
	public class InputSignatureChunk : DxbcChunk
	{
		public static InputSignatureChunk Parse(BytecodeReader reader)
		{
			return new InputSignatureChunk();
		}
	}
}