using SlimShader.ObjectModel;

namespace SlimShader
{
	public class ShaderProgramParser
	{
		public ShaderProgram Parse(DxbcReader reader)
		{
			var tokenStart = reader.BaseStream.Position;

			var program = new ShaderProgram();

			program.Version = reader.ReadShaderVersion();
			program.Length = reader.ReadUInt32();

			var tokensEnd = tokenStart + program.Length;

			while (reader.BaseStream.Position < tokensEnd)
			{
				var opcodeHeader = reader.ReadOpcodeHeader();

				var opcodeToken = new OpcodeToken();
				opcodeToken.Header = opcodeHeader;

				program.Tokens.Add(opcodeToken);

				reader.ReadUInt32();
				if (opcodeHeader.IsExtended)
					reader.ReadUInt32();
			}

			return program;
		}
	}
}