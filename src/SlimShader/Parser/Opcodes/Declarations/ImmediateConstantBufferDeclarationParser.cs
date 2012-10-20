using SlimShader.IO;
using SlimShader.ObjectModel.Tokens;

namespace SlimShader.Parser.Opcodes.Declarations
{
	/// <summary>
	/// Immediate Constant Buffer Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_CUSTOMDATA
	/// [31:11] == D3D10_SB_CUSTOMDATA_DCL_IMMEDIATE_CONSTANT_BUFFER
	///
	/// OpcodeToken0 is followed by:
	/// (1) DWORD indicating length of declaration, including OpcodeToken0.
	///     This length must = 2(for OpcodeToken0 and 1) + a multiple of 4 
	///                                                    (# of immediate constants)
	/// (2) Sequence of 4-tuples of DWORDs defining the Immediate Constant Buffer.
	///     The number of 4-tuples is (length above - 1) / 4
	/// </summary>
	public class ImmediateConstantBufferDeclarationParser : BytecodeParser<ImmediateConstantBufferDeclarationToken>
	{
		public ImmediateConstantBufferDeclarationParser(BytecodeReader reader)
			: base(reader)
		{
			
		}

		public override ImmediateConstantBufferDeclarationToken Parse()
		{
			var token0 = Reader.ReadUInt32();
			var length = Reader.ReadUInt32() - 2;

			var result = new ImmediateConstantBufferDeclarationToken
			{
				DeclarationLength = length,
				Data = new uint[length]
			};

			for (int i = 0; i < length; i++)
				result.Data[i] = Reader.ReadUInt32();

			return result;
		}
	}
}