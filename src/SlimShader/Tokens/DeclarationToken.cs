using System;
using SlimShader.IO;

namespace SlimShader.Tokens
{
	public abstract class DeclarationToken : OpcodeToken
	{
		public Operand Operand { get; internal set; }

		public static DeclarationToken Parse(BytecodeReader reader, OpcodeType opcodeType)
		{
			switch (opcodeType)
			{
				case OpcodeType.DclGlobalFlags:
					return GlobalFlagsDeclarationToken.Parse(reader);
				case OpcodeType.DclResource:
					return ResourceDeclarationToken.Parse(reader);
				case OpcodeType.DclSampler:
					return SamplerDeclarationToken.Parse(reader);
				case OpcodeType.DclInput:
				case OpcodeType.DclInputSgv:
				case OpcodeType.DclInputSiv:
				case OpcodeType.DclInputPs:
				case OpcodeType.DclInputPsSgv:
				case OpcodeType.DclInputPsSiv:
					return InputRegisterDeclarationToken.Parse(reader);
				case OpcodeType.DclOutput:
				case OpcodeType.DclOutputSgv:
				case OpcodeType.DclOutputSiv:
					return OutputRegisterDeclarationToken.Parse(reader);
				case OpcodeType.DclIndexRange:
					return IndexingRangeDeclarationToken.Parse(reader);
				case OpcodeType.DclTemps:
					return TempRegisterDeclarationToken.Parse(reader);
				case OpcodeType.DclIndexableTemp:
					return IndexableTempRegisterDeclarationToken.Parse(reader);
				case OpcodeType.DclConstantBuffer:
					return ConstantBufferDeclarationToken.Parse(reader);
				case OpcodeType.DclGsInputPrimitive:
					return GeometryShaderInputPrimitiveDeclarationToken.Parse(reader);
				case OpcodeType.DclGsOutputPrimitiveTopology:
					return GeometryShaderOutputPrimitiveTopologyDeclarationToken.Parse(reader);
				case OpcodeType.DclMaxOutputVertexCount:
					return GeometryShaderMaxOutputVertexCountDeclarationToken.Parse(reader);
				case OpcodeType.DclGsInstanceCount:
					return GeometryShaderInstanceCountDeclarationToken.Parse(reader);
				case OpcodeType.DclInputControlPointCount:
				case OpcodeType.DclOutputControlPointCount:
					return ControlPointCountDeclarationToken.Parse(reader);
				case OpcodeType.DclTessDomain:
					return TessellatorDomainDeclarationToken.Parse(reader);
				case OpcodeType.DclTessPartitioning:
					return TessellatorPartitioningDeclarationToken.Parse(reader);
				case OpcodeType.DclTessOutputPrimitive:
					return TessellatorOutputPrimitiveDeclarationToken.Parse(reader);
				case OpcodeType.DclHsMaxTessFactor:
					return HullShaderMaxTessFactorDeclarationToken.Parse(reader);
				case OpcodeType.DclHsForkPhaseInstanceCount:
					return HullShaderForkPhaseInstanceCountDeclarationToken.Parse(reader);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}