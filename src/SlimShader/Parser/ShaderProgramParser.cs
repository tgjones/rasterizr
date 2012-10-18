using SlimShader.IO;
using SlimShader.ObjectModel;
using SlimShader.ObjectModel.Tokens;
using SlimShader.Parser.Opcodes.Declarations;

namespace SlimShader.Parser
{

	public class ShaderProgramParser : BytecodeParser<ShaderProgram>
	{
		public ShaderProgramParser(BytecodeReader reader)
			: base(reader)
		{
			
		}

		public override ShaderProgram Parse()
		{
			var headerReader = Reader.CopyAtCurrentPosition();
			var program = new ShaderProgram();

			// Version Token (VerTok)
			// [07:00] minor version number (0-255)
			// [15:08] major version number (0-255)
			// [31:16] D3D10_SB_TOKENIZED_PROGRAM_TYPE
			uint versionToken = headerReader.ReadUInt32();
			program.Version = new ShaderVersion
			{
				MinorVersion = versionToken.DecodeValue<byte>(0, 3),
				MajorVersion = versionToken.DecodeValue<byte>(4, 7),
				ProgramType = versionToken.DecodeValue<ProgramType>(16, 31)
			};

			// Length Token (LenTok)
			// Always follows VerTok
			// [31:00] Unsigned integer count of number of DWORDs in program code, including version and length tokens.
			// So the minimum value is 0x00000002 (if an empty program is ever valid).
			program.Length = headerReader.ReadUInt32();

			while (!Reader.EndOfBuffer)
			{
				// Opcode Format (OpcodeToken0)
				//
				// [10:00] D3D10_SB_OPCODE_TYPE
				// if( [10:00] == D3D10_SB_OPCODE_CUSTOMDATA )
				// {
				//    Token starts a custom-data block.  See "Custom-Data Block Format".
				// }
				// else // standard opcode token
				// {
				//    [23:11] Opcode-Specific Controls
				//    [30:24] Instruction length in DWORDs including the opcode token.
				//    [31]    0 normally. 1 if extended operand definition, meaning next DWORD
				//            contains extended opcode token.
				// }
				var opcodeHeaderReader = Reader.CopyAtCurrentPosition();
				var opcodeToken0 = opcodeHeaderReader.ReadUInt32();
				var opcodeHeader = new OpcodeHeader
				{
					OpcodeType = opcodeToken0.DecodeValue<OpcodeType>(0, 10),
					Length = opcodeToken0.DecodeValue(24, 30),
					IsExtended = (opcodeToken0.DecodeValue(31, 31) == 1)
				};

				OpcodeToken opcodeToken = null;
				switch (opcodeHeader.OpcodeType)
				{
					case OpcodeType.SM4_OPCODE_DCL_GLOBAL_FLAGS:
						opcodeToken = new GlobalFlagsDeclarationParser(Reader).Parse();
						break;
					case OpcodeType.SM4_OPCODE_DCL_RESOURCE:
						opcodeToken = new ResourceDeclarationParser(Reader).Parse();
						break;
					case OpcodeType.SM4_OPCODE_DCL_SAMPLER:
						opcodeToken = new SamplerDeclarationParser(Reader).Parse();
						break;
					case OpcodeType.SM4_OPCODE_DCL_INPUT:
					case OpcodeType.SM4_OPCODE_DCL_INPUT_PS:
						opcodeToken = new InputRegisterDeclarationParser(Reader).Parse();
						break;
					//case OpcodeType.SM4_OPCODE_DCL_INPUT_SIV:
					//case OpcodeType.SM4_OPCODE_DCL_INPUT_SGV:
					//case OpcodeType.SM4_OPCODE_DCL_INPUT_PS_SIV:
					//case OpcodeType.SM4_OPCODE_DCL_INPUT_PS_SGV:
						//{
						//	opcodeToken = new InputRegisterDeclarationParser(_reader).Parse();
						//	break;
						//}
					default:
						Reader.ReadUInt32();
						if (opcodeHeader.IsExtended)
							Reader.ReadUInt32();
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
	}
}