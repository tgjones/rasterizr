using SlimShader.IO;
using SlimShader.ObjectModel;
using SlimShader.ObjectModel.Tokens;

namespace SlimShader.Parser.Opcodes.Declarations
{
	/// <summary>
	/// Hull Shader Declaration Phase: Tessellator Domain
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_TESS_DOMAIN
	/// [12:11] Domain
	/// [23:13] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	/// </summary>
	public class TessellatorDomainDeclarationParser
		: BytecodeParser<TessellatorDomainDeclarationToken>
	{
		public TessellatorDomainDeclarationParser(BytecodeReader reader)
			: base(reader)
		{
			
		}

		public override TessellatorDomainDeclarationToken Parse()
		{
			var token0 = Reader.ReadUInt32();
			return new TessellatorDomainDeclarationToken
			{
				Domain = token0.DecodeValue<TessellatorDomain>(11, 12)
			};
		}
	}
}