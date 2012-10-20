using SlimShader.IO;
using SlimShader.ObjectModel;
using SlimShader.ObjectModel.Tokens;

namespace SlimShader.Parser.Opcodes.Declarations
{
	/// <summary>
	/// Geometry Shader Input Primitive Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_GS_INPUT_PRIMITIVE
	/// [16:11] D3D10_SB_PRIMITIVE [not D3D10_SB_PRIMITIVE_TOPOLOGY]
	/// [23:17] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	/// </summary>
	public class GeometryShaderInputPrimitiveDeclarationParser : BytecodeParser<GeometryShaderInputPrimitiveDeclarationToken>
	{
		public GeometryShaderInputPrimitiveDeclarationParser(BytecodeReader reader)
			: base(reader)
		{
			
		}

		public override GeometryShaderInputPrimitiveDeclarationToken Parse()
		{
			var token0 = Reader.ReadUInt32();
			return new GeometryShaderInputPrimitiveDeclarationToken
			{
				Primitive = token0.DecodeValue<Primitive>(11, 16)
			};
		}
	}
}