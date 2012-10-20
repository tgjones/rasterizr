using SlimShader.IO;
using SlimShader.ObjectModel.Tokens;

namespace SlimShader.Parser.Opcodes.Declarations
{
	/// <summary>
	/// Hull Shader Declaration Phase: Hull Shader Max Tessfactor
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_HS_MAX_TESSFACTOR
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by a float32 representing the
	/// maximum TessFactor.
	/// </summary>
	public class HullShaderMaxTessFactorDeclarationParser
		: BytecodeParser<HullShaderMaxTessFactorDeclarationToken>
	{
		public HullShaderMaxTessFactorDeclarationParser(BytecodeReader reader)
			: base(reader)
		{
			
		}

		public override HullShaderMaxTessFactorDeclarationToken Parse()
		{
			var token0 = Reader.ReadUInt32();
			return new HullShaderMaxTessFactorDeclarationToken
			{
				MaxTessFactor = Reader.ReadSingle()
			};
		}
	}
}