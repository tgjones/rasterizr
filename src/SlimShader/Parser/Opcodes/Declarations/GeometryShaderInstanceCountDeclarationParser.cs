using SlimShader.IO;
using SlimShader.ObjectModel;
using SlimShader.ObjectModel.Tokens;

namespace SlimShader.Parser.Opcodes.Declarations
{
	/// <summary>
	/// Geometry Shader Instance Count Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_GS_INSTANCE_COUNT
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by a UINT32 representing the
	/// number of instances of the geometry shader program to execute.
	/// </summary>
	public class GeometryShaderInstanceCountDeclarationParser
		: BytecodeParser<GeometryShaderInstanceCountDeclarationToken>
	{
		public GeometryShaderInstanceCountDeclarationParser(BytecodeReader reader)
			: base(reader)
		{
			
		}

		public override GeometryShaderInstanceCountDeclarationToken Parse()
		{
			var token0 = Reader.ReadUInt32();
			return new GeometryShaderInstanceCountDeclarationToken
			{
				InstanceCount = Reader.ReadUInt32()
			};
		}
	}
}