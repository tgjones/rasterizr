using System.ComponentModel;

namespace SlimShader.Shader
{
	public enum OpcodeType
	{
		[Description("add")]
		Add,

		[Description("and")]
		And,

		[Description("break")]
		Break,

		[Description("breakc")]
		BreakC,

		[Description("call")]
		Call,

		[Description("callc")]
		CallC,

		[Description("case")]
		Case,

		[Description("continue")]
		Continue,

		[Description("continuec")]
		ContinueC,

		[Description("cut")]
		Cut,

		[Description("default")]
		Default,

		[Description("deriv_rtx")]
		DerivRtx,

		[Description("deriv_rty")]
		DerivRty,

		[Description("discard")]
		Discard,

		[Description("div")]
		Div,

		[Description("dp2")]
		Dp2,

		[Description("dp3")]
		Dp3,

		[Description("dp4")]
		Dp4,

		[Description("else")]
		Else,

		[Description("emit")]
		Emit,

		[Description("emitThenCut")]
		EmitThenCut,

		[Description("endif")]
		EndIf,

		[Description("endloop")]
		EndLoop,

		[Description("endswitch")]
		EndSwitch,

		[Description("eq")]
		Eq,

		[Description("exp")]
		Exp,

		[Description("frc")]
		Frc,

		[Description("ftoi")]
		FtoI,

		[Description("ftou")]
		FtoU,

		[Description("ge")]
		Ge,

		[Description("iadd")]
		IAdd,

		[Description("if")]
		If,

		[Description("ieq")]
		IEq,

		[Description("ige")]
		IGe,

		[Description("ilt")]
		ILt,

		[Description("imad")]
		IMad,

		[Description("imax")]
		IMax,

		[Description("imin")]
		IMin,

		[Description("imul")]
		IMul,

		[Description("ine")]
		INe,

		[Description("ineg")]
		INeg,

		[Description("ishl")]
		IShl,

		[Description("ishr")]
		IShr,

		[Description("itof")]
		IToF,

		[Description("label")]
		Label,

		[Description("ld")]
		Ld,

		[Description("ldms")]
		LdMs,

		[Description("log")]
		Log,

		[Description("loop")]
		Loop,

		[Description("lt")]
		Lt,

		[Description("mad")]
		Mad,

		[Description("min")]
		Min,

		[Description("max")]
		Max,

		CustomData,

		[Description("mov")]
		Mov,

		[Description("movc")]
		MovC,

		[Description("mul")]
		Mul,

		[Description("ne")]
		Ne,

		[Description("nop")]
		Nop,

		[Description("not")]
		Not,

		[Description("or")]
		Or,

		[Description("resinfo")]
		Resinfo,

		[Description("ret")]
		Ret,

		[Description("retc")]
		RetC,

		[Description("round_ne")]
		RoundNe,

		[Description("round_ni")]
		RoundNi,

		[Description("round_pi")]
		RoundPi,

		[Description("round_z")]
		RoundZ,

		[Description("rsq")]
		Rsq,

		[Description("sample")]
		Sample,

		[Description("sample_c")]
		SampleC,

		[Description("sample_c_lz")]
		SampleCLz,

		[Description("sample_l")]
		SampleL,

		[Description("sample_d")]
		SampleD,

		[Description("sample_b")]
		SampleB,

		[Description("sqrt")]
		Sqrt,

		[Description("switch")]
		Switch,

		[Description("sincos")]
		Sincos,

		[Description("udiv")]
		UDiv,

		[Description("ult")]
		ULt,

		[Description("uge")]
		UGe,

		[Description("umul")]
		UMul,

		[Description("umad")]
		UMad,

		[Description("umax")]
		UMax,

		[Description("umin")]
		UMin,

		[Description("ushr")]
		UShr,

		[Description("utof")]
		UTof,

		[Description("xor")]
		Xor,

		[Description("dcl_resource")]
		DclResource,

		[Description("dcl_constantbuffer")]
		DclConstantBuffer,

		[Description("dcl_sampler")]
		DclSampler,

		[Description("dcl_indexrange")]
		DclIndexRange,

		[Description("dcl_outputtopology")]
		DclGsOutputPrimitiveTopology,

		[Description("dcl_inputprimitive")]
		DclGsInputPrimitive,

		[Description("dcl_maxout")]
		DclMaxOutputVertexCount,

		[Description("dcl_input")]
		DclInput,

		[Description("dcl_input_sgv")]
		DclInputSgv,

		[Description("dcl_input_siv")]
		DclInputSiv,

		[Description("dcl_input_ps")]
		DclInputPs,

		[Description("dcl_input_ps_sgv")]
		DclInputPsSgv,

		[Description("dcl_input_ps_siv")]
		DclInputPsSiv,

		[Description("dcl_output")]
		DclOutput,

		[Description("dcl_output_sgv")]
		DclOutputSgv,

		[Description("dcl_output_siv")]
		DclOutputSiv,

		[Description("dcl_temps")]
		DclTemps,

		[Description("dcl_indexableTemp")]
		DclIndexableTemp,

		[Description("dcl_globalFlags")]
		DclGlobalFlags,

		/// <summary>
		/// This marks the end of D3D10.0 opcodes
		/// </summary>
		D3D10Count,

		// DX 10.1 op codes

		[Description("lod")]
		Lod,

		[Description("gather4")]
		Gather4,

		[Description("samplepos")]
		SamplePos,

		[Description("sampleinfo")]
		SampleInfo,

		/// <summary>
		/// This marks the end of D3D10.1 opcodes
		/// </summary>
		D3D10_1Count,

		// DX 11 op codes

		/// <summary>
		/// Token marks beginning of HS sub-shader
		/// </summary>
		[Description("hs_decls")]
		HsDecls,

		/// <summary>
		/// Token marks beginning of HS sub-shader
		/// </summary>
		[Description("hs_control_point_phase")]
		HsControlPointPhase,

		/// <summary>
		/// Token marks beginning of HS sub-shader
		/// </summary>
		[Description("hs_fork_phase")]
		HsForkPhase,

		/// <summary>
		/// Token marks beginning of HS sub-shader
		/// </summary>
		[Description("hs_join_phase")]
		HsJoinPhase,

		[Description("emit_stream")]
		EmitStream,

		[Description("cut_stream")]
		CutStream,

		// TODO: Finish descriptions.
		EmitThenCutStream,

		[Description("fcall")]
		InterfaceCall,
		Bufinfo,
		RtxCoarse,
		RtxFine,
		RtyCoarse,
		RtyFine,
		Gather4C,
		Gather4Po,
		Gather4PoC,
		Rcp,
		F32ToF16,
		F16ToF32,
		UAddC,
		USubB,
		CountBits,
		FirstBitHi,
		FirstBitLo,
		FirstBitSHi,
		UBfe,
		IBfe,
		Bfi,
		BfRev,
		SwapC,
		DclStream,

		[Description("dcl_function_body")]
		DclFunctionBody,

		[Description("dcl_function_table")]
		DclFunctionTable,

		[Description("dcl_interface")]
		DclInterface,

		[Description("dcl_input_control_point_count")]
		DclInputControlPointCount,

		[Description("dcl_output_control_point_count")]
		DclOutputControlPointCount,

		[Description("dcl_tessellator_domain")]
		DclTessDomain,

		[Description("dcl_tessellator_partitioning")]
		DclTessPartitioning,

		[Description("dcl_tessellator_output_primitive")]
		DclTessOutputPrimitive,
		DclHsMaxTessFactor,

		[Description("dcl_hs_fork_phase_instance_count")]
		DclHsForkPhaseInstanceCount,

		[Description("dcl_hs_join_phase_instance_count")]
		DclHsJoinPhaseInstanceCount,

		DclThreadGroup,
		DclUnorderedAccessViewTyped,
		DclUnorderedAccessViewRaw,
		DclUnorderedAccessViewStructured,
		DclThreadGroupSharedMemoryRaw,
		DclThreadGroupSharedMemoryStructured,
		DclResourceRaw,
		DclResourceStructured,
		LdUavTyped,
		StoreUavTyped,
		LdRaw,
		StoreRaw,
		LdStructured,
		StoreStructured,
		AtomicAnd,
		AtomicOr,
		AtomicXor,
		AtomicCmpStore,
		AtomicIAdd,
		AtomicIMax,
		AtomicIMin,
		AtomicUMax,
		AtomicUMin,
		ImmAtomicAlloc,
		ImmAtomicConsume,
		ImmAtomicIAdd,
		ImmAtomicAnd,
		ImmAtomicOr,
		ImmAtomicXor,
		ImmAtomicExch,
		ImmAtomicCmpExch,
		ImmAtomicIMax,
		ImmAtomicIMin,
		ImmAtomicUMax,
		ImmAtomicUMin,
		Sync,
		DAdd,
		DMax,
		DMin,
		DMul,
		DEq,
		DGe,
		DLt,
		DNe,
		DMov,
		DMovC,
		DToD,
		FToD,
		EvalSnapped,
		EvalSampleIndex,
		EvalCentroid,
		DclGsInstanceCount,
		Abort,
		DebugBreak,

		/// <summary>
		/// This marks the end of D3D11.0 opcodes
		/// </summary>
		D3D11_0Count,

		Ddiv,
		Dfma,
		Drcp,

		Msad,

		Dtoi,
		Dtou,
		Itod,
		Utod
	};
}