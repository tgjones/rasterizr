using SlimShader.IO;
using SlimShader.ObjectModel.Tokens;

namespace SlimShader.Parser.Opcodes.Declarations
{
	/// <summary>
	/// Hull Shader Declaration Phase: HS/DS Input Control Point Count
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_INPUT_CONTROL_POINT_COUNT
	/// [16:11] Control point count 
	/// [23:17] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	/// 
	/// -------
	/// 
	/// Hull Shader Declaration Phase: HS Output Control Point Count
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_OUTPUT_CONTROL_POINT_COUNT
	/// [16:11] Control point count 
	/// [23:17] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	/// </summary>
	public class ControlPointCountDeclarationParser
		: BytecodeParser<ControlPointCountDeclarationToken>
	{
		public ControlPointCountDeclarationParser(BytecodeReader reader)
			: base(reader)
		{
			
		}

		public override ControlPointCountDeclarationToken Parse()
		{
			var token0 = Reader.ReadUInt32();
			return new ControlPointCountDeclarationToken
			{
				ControlPointCount = token0.DecodeValue(11, 16)
			};
		}
	}
}