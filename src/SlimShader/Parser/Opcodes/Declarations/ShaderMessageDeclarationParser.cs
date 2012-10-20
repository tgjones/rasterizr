using SlimShader.IO;
using SlimShader.ObjectModel;
using SlimShader.ObjectModel.Tokens;

namespace SlimShader.Parser.Opcodes.Declarations
{
	/// <summary>
	/// Shader Message Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_CUSTOMDATA
	/// [31:11] == D3D11_SB_CUSTOMDATA_SHADER_MESSAGE
	///
	/// OpcodeToken0 is followed by:
	/// (1) DWORD indicating length of declaration, including OpcodeToken0.
	/// (2) DWORD indicating the info queue message ID.
	/// (3) D3D11_SB_SHADER_MESSAGE_FORMAT indicating the convention for formatting the message.
	/// (4) DWORD indicating the number of characters in the string without the terminator.
	/// (5) DWORD indicating the number of operands.
	/// (6) DWORD indicating length of operands.
	/// (7) Encoded operands.
	/// (8) String with trailing zero, padded to a multiple of DWORDs.
	///     The string is in the given format and the operands given should
	///     be used for argument substitutions when formatting.
	/// </summary>
	public class ShaderMessageDeclarationParser : BytecodeParser<ShaderMessageDeclarationToken>
	{
		public ShaderMessageDeclarationParser(BytecodeReader reader)
			: base(reader)
		{
			
		}

		public override ShaderMessageDeclarationToken Parse()
		{
			var token0 = Reader.ReadUInt32();
			var length = Reader.ReadUInt32() - 2;

			var result = new ShaderMessageDeclarationToken
			{
				DeclarationLength = length,
				InfoQueueMessageID = Reader.ReadUInt32(),
				MessageFormat = (ShaderMessageFormat) Reader.ReadUInt32(),
				NumCharacters = Reader.ReadUInt32(),
				NumOperands = Reader.ReadUInt32(),
				OperandsLength = Reader.ReadUInt32()
			};

			// TODO: Read encoded operands and format string.
			for (int i = 0; i < length - 5; i++)
				Reader.ReadUInt32();

			return result;
		}
	}
}