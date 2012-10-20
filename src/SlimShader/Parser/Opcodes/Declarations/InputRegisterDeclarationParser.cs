using System;
using SlimShader.IO;
using SlimShader.ObjectModel;
using SlimShader.ObjectModel.Tokens;

namespace SlimShader.Parser.Opcodes.Declarations
{
	/// <summary>
	/// Input Register Declaration (see separate declarations for Pixel Shaders)
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INPUT
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 1 operand:
	/// (1) Operand, starting with OperandToken0, defining which input
	///     v# register (D3D10_SB_OPERAND_TYPE_INPUT) is being declared, 
	///     including writemask.
	/// 
	/// -------
	/// 
	/// Input Register Declaration w/System Interpreted Value
	/// (see separate declarations for Pixel Shaders)
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INPUT_SIV
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) Operand, starting with OperandToken0, defining which input
	///     v# register (D3D10_SB_OPERAND_TYPE_INPUT) is being declared,
	///     including writemask.  For Geometry Shaders, the input is 
	///     v[vertex][attribute], and this declaration is only for which register 
	///     on the attribute axis is being declared.  The vertex axis value must 
	///     be equal to the # of vertices in the current input primitive for the GS
	///     (i.e. 6 for triangle + adjacency).
	/// (2) a System Interpreted Value Name (NameToken)
	/// 
	/// -------
	/// 
	/// Input Register Declaration w/System Generated Value
	/// (available for all shaders incl. Pixel Shader, no interpolation mode needed)
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INPUT_SGV
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) Operand, starting with OperandToken0, defining which input
	///     v# register (D3D10_SB_OPERAND_TYPE_INPUT) is being declared,
	///     including writemask.
	/// (2) a System Generated Value Name (NameToken)
	/// 
	/// -------
	/// 
	/// Pixel Shader Input Register Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INPUT_PS
	/// [14:11] D3D10_SB_INTERPOLATION_MODE
	/// [23:15] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 1 operand:
	/// (1) Operand, starting with OperandToken0, defining which input
	///     v# register (D3D10_SB_OPERAND_TYPE_INPUT) is being declared,
	///     including writemask.
	/// 
	/// -------
	/// 
	/// Pixel Shader Input Register Declaration w/System Interpreted Value
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INPUT_PS_SIV
	/// [14:11] D3D10_SB_INTERPOLATION_MODE
	/// [23:15] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) Operand, starting with OperandToken0, defining which input
	///     v# register (D3D10_SB_OPERAND_TYPE_INPUT) is being declared.
	/// (2) a System Interpreted Value Name (NameToken)
	/// 
	/// -------
	/// 
	/// Pixel Shader Input Register Declaration w/System Generated Value
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INPUT_PS_SGV
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) Operand, starting with OperandToken0, defining which input
	///     v# register (D3D10_SB_OPERAND_TYPE_INPUT) is being declared.
	/// (2) a System Generated Value Name (NameToken)
	/// </summary>
	public class InputRegisterDeclarationParser : BytecodeParser<InputRegisterDeclarationToken>
	{
		public InputRegisterDeclarationParser(BytecodeReader reader)
			: base(reader)
		{
			
		}

		public override InputRegisterDeclarationToken Parse()
		{
			uint token0 = Reader.ReadUInt32();
			var opcodeType = token0.DecodeValue<OpcodeType>(0, 10);

			InputRegisterDeclarationToken result;
			switch (opcodeType)
			{
				case OpcodeType.DclInput :
				case OpcodeType.DclInputSgv:
				case OpcodeType.DclInputSiv:
					result = new InputRegisterDeclarationToken();
					break;
				case OpcodeType.DclInputPs:
				case OpcodeType.DclInputPsSgv:
				case OpcodeType.DclInputPsSiv:
					result = new PixelShaderInputRegisterDeclarationToken
					{
						InterpolationMode = token0.DecodeValue<InterpolationMode>(11, 14)
					};
					break;
				default :
					throw new ArgumentOutOfRangeException();
			}

			result.Operand = new OperandParser(Reader, false).Parse();

			switch (opcodeType)
			{
				case OpcodeType.DclInputSgv:
				case OpcodeType.DclInputSiv:
				case OpcodeType.DclInputPsSgv:
				case OpcodeType.DclInputPsSiv:
					result.SystemValueName = new NameTokenParser(Reader).Parse();
					break;
			}

			return result;
		}
	}
}