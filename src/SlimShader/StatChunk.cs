using SlimShader.IO;

namespace SlimShader
{
	// TODO: I don't know what this is...
	public class StatChunk : DxbcChunk
	{
		public static StatChunk Parse(BytecodeReader reader)
		{
			return new StatChunk();
		}
	}
}