using System.Collections.Generic;
using SlimShader.IO;

namespace SlimShader.InputOutputSignature
{
	public class InputOutputSignatureChunk : DxbcChunk
	{
		public List<SignatureParameterDescription> Parameters { get; private set; }

		public InputOutputSignatureChunk()
		{
			Parameters = new List<SignatureParameterDescription>();
		}

		public static InputOutputSignatureChunk Parse(BytecodeReader reader)
		{
			var result = new InputOutputSignatureChunk();

			var chunkReader = reader.CopyAtCurrentPosition();
			var elementCount = chunkReader.ReadUInt32();
			var uniqueKey = chunkReader.ReadUInt32();

			for (int i = 0; i < elementCount; i++)
				result.Parameters.Add(SignatureParameterDescription.Parse(reader, chunkReader));

			return result;
		}
	}
}