using SlimShader.IO;

namespace SlimShader.ObjectModel
{
	public class InputSignatureChunk : DxbcChunk
	{
		public static InputSignatureChunk Parse(BytecodeReader reader)
		{
			return new InputSignatureChunk();
		}
	}
}