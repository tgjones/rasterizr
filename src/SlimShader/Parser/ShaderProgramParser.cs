using SlimShader.ObjectModel;
using SlimShader.ObjectModel.Tokens;
using SlimShader.Parser.Declarations;

namespace SlimShader.Parser
{
	public class ShaderProgramParser : DxbcParser
	{
		private readonly DxbcReader _reader;

		public ShaderProgramParser(DxbcReader reader)
		{
			_reader = reader;
		}

		public ShaderProgram Parse()
		{
			var tokenStart = _reader.CurrentIndex;

			var program = new ShaderProgram();

			program.Version = ReadShaderVersion();
			program.Length = Decoder.DecodeProgramLength(_reader.ReadAndMoveNext());

			var tokensEnd = tokenStart + program.Length;

			while (_reader.CurrentIndex < tokensEnd)
			{
				var opcodeHeader = ReadOpcodeHeader();

				OpcodeToken opcodeToken = null;
				switch (opcodeHeader.OpcodeType)
				{
					case OpcodeType.SM4_OPCODE_DCL_GLOBAL_FLAGS:
						opcodeToken = new DeclareGlobalFlagsToken();
						_reader.MoveNext();
						break;
					case OpcodeType.SM4_OPCODE_DCL_RESOURCE:
					{
						opcodeToken = new ResourceDeclarationParser(_reader).Parse();
						break;
					}
					case OpcodeType.SM4_OPCODE_DCL_SAMPLER:
					{
						opcodeToken = new SamplerDeclarationParser(_reader).Parse();
						break;
					}
					default:
						_reader.MoveNext();
						if (opcodeHeader.IsExtended)
							_reader.MoveNext();
						break;
				}

				if (opcodeToken != null)
				{
					opcodeToken.Header = opcodeHeader;
					program.Tokens.Add(opcodeToken);
				}
			}

			return program;
		}

		private ShaderVersion ReadShaderVersion()
		{
			uint token = _reader.ReadAndMoveNext();
			return new ShaderVersion
			{
				MajorVersion = Decoder.DecodeProgramMajorVersion(token),
				MinorVersion = Decoder.DecodeProgramMinorVersion(token),
				ProgramType = Decoder.DecodeProgramType(token)
			};
		}

		private OpcodeHeader ReadOpcodeHeader()
		{
			return new OpcodeHeader
			{
				OpcodeType = Decoder.DecodeOpcodeType(_reader.CurrentToken),
				Length = Decoder.DecodeInstructionLength(_reader.CurrentToken),
				IsExtended = Decoder.DecodeIsOpcodeExtended(_reader.CurrentToken)
			};
		}

		

		
	}
}