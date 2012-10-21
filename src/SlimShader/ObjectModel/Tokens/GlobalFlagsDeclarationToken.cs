using SlimShader.IO;
using SlimShader.Parser;

namespace SlimShader.ObjectModel.Tokens
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
	public class GlobalFlagsDeclarationToken : DeclarationToken
	{
		public bool RefactoringAllowed { get; internal set; }

		public static GlobalFlagsDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new GlobalFlagsDeclarationToken
			{
				RefactoringAllowed = (token0.DecodeValue(11, 11) == 1)
			};
		}
	}
}