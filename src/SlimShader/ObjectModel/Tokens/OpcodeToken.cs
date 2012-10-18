namespace SlimShader.ObjectModel.Tokens
{
	public abstract class OpcodeToken
	{
		public OpcodeHeader Header { get; internal set; }
		public Operand Operand { get; internal set; }
	}

	public abstract class DeclarationToken : OpcodeToken
	{
		
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