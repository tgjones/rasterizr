using SlimShader.IO;
using SlimShader.ObjectModel;
using SlimShader.ObjectModel.Tokens;

namespace SlimShader.Parser.Opcodes.Declarations
{
	/// <summary>
	/// Geometry Shader Output Topology Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_GS_OUTPUT_PRIMITIVE_TOPOLOGY
	/// [17:11] D3D10_SB_PRIMITIVE_TOPOLOGY
	/// [23:18] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	/// </summary>
	public class GeometryShaderOutputPrimitiveTopologyDeclarationParser
		: BytecodeParser<GeometryShaderOutputPrimitiveTopologyDeclarationToken>
	{
		public GeometryShaderOutputPrimitiveTopologyDeclarationParser(BytecodeReader reader)
			: base(reader)
		{
			
		}

		public override GeometryShaderOutputPrimitiveTopologyDeclarationToken Parse()
		{
			var token0 = Reader.ReadUInt32();
			return new GeometryShaderOutputPrimitiveTopologyDeclarationToken
			{
				PrimitiveTopology = token0.DecodeValue<PrimitiveTopology>(11, 17)
			};
		}
	}
}