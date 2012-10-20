namespace SlimShader.ObjectModel.Tokens
{
	/// <summary>
	/// Sampler Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_SAMPLER
	/// [14:11] D3D10_SB_SAMPLER_MODE
	/// [23:15] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 1 operand:
	/// (1) Operand starting with OperandToken0, defining which sampler
	///     (D3D10_SB_OPERAND_TYPE_SAMPLER) register # is being declared.
	/// </summary>
	public class SamplerDeclarationToken : DeclarationToken
	{
		public SamplerMode SamplerMode { get; internal set; }

		public override string ToString()
		{
			return string.Format("{0} s{1}, {2}", TypeDescription, Operand.Indices[0].Value,
				SamplerMode.GetDescription());
		}
	}
}