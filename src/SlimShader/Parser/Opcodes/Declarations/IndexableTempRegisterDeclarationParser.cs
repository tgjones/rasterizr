using SlimShader.IO;
using SlimShader.ObjectModel.Tokens;

namespace SlimShader.Parser.Opcodes.Declarations
{
	/// <summary>
	/// Indexable Temp Register (x#[size]) Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INDEXABLE_TEMP
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 3 DWORDs:
	/// (1) Register index (defines which x# register is declared)
	/// (2) Number of registers in this register bank
	/// (3) Number of components in the array (1-4). 1 means .x, 2 means .xy etc.
	/// </summary>
	public class IndexableTempRegisterDeclarationParser : BytecodeParser<IndexableTempRegisterDeclarationToken>
	{
		public IndexableTempRegisterDeclarationParser(BytecodeReader reader)
			: base(reader)
		{
			
		}

		public override IndexableTempRegisterDeclarationToken Parse()
		{
			var token0 = Reader.ReadUInt32();
			return new IndexableTempRegisterDeclarationToken
			{
				RegisterIndex = Reader.ReadUInt32(),
				RegisterCount = Reader.ReadUInt32(),
				NumComponents = Reader.ReadUInt32()
			};
		}
	}
}