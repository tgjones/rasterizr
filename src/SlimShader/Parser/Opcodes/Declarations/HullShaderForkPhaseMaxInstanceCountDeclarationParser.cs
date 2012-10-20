using SlimShader.IO;
using SlimShader.ObjectModel.Tokens;

namespace SlimShader.Parser.Opcodes.Declarations
{
	/// <summary>
	/// Hull Shader Declaration Phase: Hull Shader Fork Phase Instance Count
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_HS_FORK_PHASE_INSTANCE_COUNT
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by a UINT32 representing the
	/// number of instances of the current fork phase program to execute.
	/// </summary>
	public class HullShaderForkPhaseInstanceCountDeclarationParser
		: BytecodeParser<HullShaderForkPhaseInstanceCountDeclarationToken>
	{
		public HullShaderForkPhaseInstanceCountDeclarationParser(BytecodeReader reader)
			: base(reader)
		{
			
		}

		public override HullShaderForkPhaseInstanceCountDeclarationToken Parse()
		{
			var token0 = Reader.ReadUInt32();
			return new HullShaderForkPhaseInstanceCountDeclarationToken
			{
				InstanceCount = Reader.ReadUInt32()
			};
		}
	}
}