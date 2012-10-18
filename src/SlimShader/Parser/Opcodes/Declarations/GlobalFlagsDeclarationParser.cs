using SlimShader.IO;
using SlimShader.ObjectModel.Tokens;

namespace SlimShader.Parser.Opcodes.Declarations
{
	/// <summary>
	/// Global Flags Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_GLOBAL_FLAGS
	/// [11:11] Refactoring allowed if bit set.
	/// [23:12] Reserved for future flags.
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by no operands.
	/// </summary>
	public class GlobalFlagsDeclarationParser : BytecodeParser<GlobalFlagsDeclarationToken>
	{
		public GlobalFlagsDeclarationParser(BytecodeReader reader)
			: base(reader)
		{
		}

		public override GlobalFlagsDeclarationToken Parse()
		{
			var token0 = Reader.ReadUInt32();
			return new GlobalFlagsDeclarationToken
			{
				RefactoringAllowed = (token0.DecodeValue(11, 11) == 1)
			};
		}
	}
}