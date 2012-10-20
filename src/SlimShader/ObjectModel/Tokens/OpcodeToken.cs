using System.Collections.Generic;

namespace SlimShader.ObjectModel.Tokens
{
	public abstract class OpcodeToken
	{
		public OpcodeHeader Header { get; internal set; }
	}

	public abstract class DeclarationToken : OpcodeToken
	{
		public Operand Operand { get; internal set; }
	}

	public class InstructionToken : OpcodeToken
	{
		public InstructionTokenExtendedType ExtendedType { get; internal set; }
		public uint[] SampleOffsets { get; private set; }
		public byte ResourceTarget { get; internal set; }
		public byte[] ResourceReturnTypes { get; private set; }
		public List<Operand> Operands { get; private set; }

		public InstructionToken()
		{
			SampleOffsets = new uint[3];
			ResourceReturnTypes = new byte[4];
			Operands = new List<Operand>();
		}
	}

	public class GlobalFlagsDeclarationToken : DeclarationToken
	{
		public bool RefactoringAllowed { get; internal set; }
	}

	public class ResourceDeclarationToken : DeclarationToken
	{
		public ResourceDimension ResourceDimension { get; internal set; }
		public byte SampleCount { get; internal set; }
		public ResourceReturnTypeToken ReturnType { get; internal set; }
	}

	public class SamplerDeclarationToken : DeclarationToken
	{
		public SamplerMode SamplerMode { get; internal set; }
	}

	public class InputRegisterDeclarationToken : DeclarationToken
	{
		/// <summary>
		/// Only applicable for SGV and SIV declarations.
		/// </summary>
		public SystemValueName SystemValueName { get; internal set; }
	}

	public class PixelShaderInputRegisterDeclarationToken : InputRegisterDeclarationToken
	{
		/// <summary>
		/// Not applicable for D3D10_SB_OPCODE_DCL_INPUT_PS_SGV
		/// </summary>
		public InterpolationMode InterpolationMode { get; set; }
	}

	public class OutputRegisterDeclarationToken : DeclarationToken
	{
		/// <summary>
		/// Only applicable for SGV and SIV declarations.
		/// </summary>
		public SystemValueName SystemValueName { get; internal set; }
	}

	public class IndexingRangeDeclarationToken : DeclarationToken
	{
		/// <summary>
		/// Represents the count of registers starting the from the one indicated in Operand.
		/// </summary>
		public uint RegisterCount { get; internal set; }
	}

	public class TempRegisterDeclarationToken : DeclarationToken
	{
		/// <summary>
		/// Indicates how many temps are being declared. i.e. 5 means r0...r4 are declared.
		/// </summary>
		public uint TempCount { get; internal set; }
	}

	public class IndexableTempRegisterDeclarationToken : DeclarationToken
	{
		/// <summary>
		/// Register index (defines which x# register is declared)
		/// </summary>
		public uint RegisterIndex { get; internal set; }

		/// <summary>
		/// Number of registers in this register bank
		/// </summary>
		public uint RegisterCount { get; internal set; }

		/// <summary>
		/// Number of components in the array (1-4). 1 means .x, 2 means .xy, etc.
		/// </summary>
		public uint NumComponents { get; internal set; }
	}

	public class ConstantBufferDeclarationToken : DeclarationToken
	{
		public ConstantBufferAccessPattern AccessPattern { get; internal set; }
	}

	public abstract class ImmediateDeclarationToken : DeclarationToken
	{
		public uint DeclarationLength { get; internal set; }
	}

	public class ImmediateConstantBufferDeclarationToken : ImmediateDeclarationToken
	{
		public uint[] Data { get; internal set; }
	}

	public class ShaderMessageDeclarationToken : ImmediateDeclarationToken
	{
		/// <summary>
		/// Indicates the info queue message ID.
		/// </summary>
		public uint InfoQueueMessageID { get; internal set; }

		/// <summary>
		/// Indicates the convention for formatting the message.
		/// </summary>
		public ShaderMessageFormat MessageFormat { get; internal set; }

		/// <summary>
		/// DWORD indicating the number of characters in the string without the terminator.
		/// </summary>
		public uint NumCharacters { get; internal set; }

		/// <summary>
		/// DWORD indicating the number of operands.
		/// </summary>
		public uint NumOperands { get; internal set; }

		/// <summary>
		/// DWORD indicating length of operands.
		/// </summary>
		public uint OperandsLength { get; internal set; }

		// Not sure what format this is.
		public object EncodedOperands { get; internal set; }

		/// <summary>
		/// String with trailing zero, padded to a multiple of DWORDs.
		/// The string is in the given format and the operands given should
		/// be used for argument substitutions when formatting.
		/// </summary>
		public string Format { get; internal set; }
	}

	public class GeometryShaderInputPrimitiveDeclarationToken : DeclarationToken
	{
		public Primitive Primitive { get; set; }
	}

	public class GeometryShaderOutputPrimitiveTopologyDeclarationToken : DeclarationToken
	{
		public PrimitiveTopology PrimitiveTopology { get; set; }
	}

	public class GeometryShaderMaxOutputVertexCountDeclarationToken : DeclarationToken
	{
		public uint MaxPrimitives { get; set; }
	}

	public class GeometryShaderInstanceCountDeclarationToken : DeclarationToken
	{
		public uint InstanceCount { get; set; }
	}

	public class ControlPointCountDeclarationToken : DeclarationToken
	{
		public uint ControlPointCount { get; set; }
	}

	public class TessellatorDomainDeclarationToken : DeclarationToken
	{
		public TessellatorDomain Domain { get; set; }
	}

	public class TessellatorPartioningDeclarationToken : DeclarationToken
	{
		public TessellatorPartitioning Partioning { get; set; }
	}

	public class TessellatorOutputPrimitiveDeclarationToken : DeclarationToken
	{
		public TessellatorOutputPrimitive OutputPrimitive { get; set; }
	}

	public class HullShaderMaxTessFactorDeclarationToken : DeclarationToken
	{
		public float MaxTessFactor { get; set; }
	}

	public class HullShaderForkPhaseInstanceCountDeclarationToken : DeclarationToken
	{
		public uint InstanceCount { get; set; }
	}

	public class ResourceReturnTypeToken
	{
		public ResourceReturnType X { get; internal set; }
		public ResourceReturnType Y { get; internal set; }
		public ResourceReturnType Z { get; internal set; }
		public ResourceReturnType W { get; internal set; }
	}

	public class Operand
	{
		public byte NumComponents { get; internal set; }
		public Operand4ComponentSelectionMode SelectionMode { get; internal set; }
		public ComponentMask ComponentMask { get; internal set; }
		public Operand4ComponentName[] Swizzles { get; private set; }
		public OperandType OperandType { get; internal set; }
		public OperandIndexDimension IndexDimension { get; internal set; }
		// TODO: Can merge this with Indices?
		public OperandIndexRepresentation[] IndexRepresentations { get; private set; }
		public bool IsExtended { get; internal set; }
		public OperandModifier Modifier { get; internal set; }
		public OperandIndex[] Indices { get; private set; }
		public ulong[] ImmediateValues { get; private set; }

		public Operand()
		{
			Swizzles = new[]
			{
				Operand4ComponentName.X,
				Operand4ComponentName.Y,
				Operand4ComponentName.Z,
				Operand4ComponentName.W
			};
			IndexRepresentations = new OperandIndexRepresentation[3];
			Indices = new OperandIndex[3];
			ImmediateValues = new ulong[4];
		}
	}

	public class OperandIndex
	{
		public ulong Value { get; set; }
		public Operand Register { get; set; }

	}
}