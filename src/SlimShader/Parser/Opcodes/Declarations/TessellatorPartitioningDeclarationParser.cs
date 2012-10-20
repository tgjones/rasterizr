using SlimShader.IO;
using SlimShader.ObjectModel;
using SlimShader.ObjectModel.Tokens;

namespace SlimShader.Parser.Opcodes.Declarations
{
	/// <summary>
	/// Hull Shader Declaration Phase: Tessellator Partitioning
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_TESS_PARTITIONING
	/// [13:11] Partitioning
	/// [23:14] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	/// </summary>
	public class TessellatorPartitioningDeclarationParser
		: BytecodeParser<TessellatorPartioningDeclarationToken>
	{
		public TessellatorPartitioningDeclarationParser(BytecodeReader reader)
			: base(reader)
		{
			
		}

		public override TessellatorPartioningDeclarationToken Parse()
		{
			var token0 = Reader.ReadUInt32();
			return new TessellatorPartioningDeclarationToken
			{
				Partioning = token0.DecodeValue<TessellatorPartitioning>(11, 13)
			};
		}
	}
}