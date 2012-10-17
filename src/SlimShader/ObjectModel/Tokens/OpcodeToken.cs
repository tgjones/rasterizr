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

	public class DeclareGlobalFlagsToken : DeclarationToken
	{
		
	}

	public class DeclareResourceToken : DeclarationToken
	{
		public ResourceDimension ResourceDimension { get; internal set; }
		public byte SampleCount { get; internal set; }
		public ResourceReturnTypeToken ReturnType { get; internal set; }
	}

	public class DeclareSamplerToken : DeclarationToken
	{
		public SamplerMode SamplerMode { get; internal set; }
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
		public bool IsExtended { get; internal set; }
		public OperandType OperandType { get; internal set; }
		public OperandModifier Modifier { get; internal set; }
		public OperandIndexDimension IndexDimension { get; internal set; }
		public int WriteMask { get; internal set; }
		public bool WriteMaskEnabled { get; internal set; }
		public int GSInput { get; internal set; }
		public uint RegisterNumber { get; internal set; }
		public int NumComponents { get; internal set; }
		public Operand4ComponentSelectionMode SelectionMode { get; internal set; }
		public uint ComponentMask { get; internal set; }
		public uint Swizzle { get; internal set; }
		public Operand4ComponentName[] Swizzles { get; private set; }
		public float[] ImmediateValues { get; private set; }
		public OperandIndexRepresentation[] IndexRepresentations { get; private set; }
		public uint[] ArraySizes { get; private set; }
		public Operand[] Suboperands { get; private set; }

		public Operand()
		{
			WriteMaskEnabled = true;
			GSInput = 0;
			Swizzles = new[]
			{
				Operand4ComponentName.D3D10_SB_4_COMPONENT_X,
				Operand4ComponentName.D3D10_SB_4_COMPONENT_Y,
				Operand4ComponentName.D3D10_SB_4_COMPONENT_Z,
				Operand4ComponentName.D3D10_SB_4_COMPONENT_W,
			};
			ImmediateValues = new float[4];
			IndexRepresentations = new OperandIndexRepresentation[3];
			ArraySizes = new uint[3];
			Suboperands = new Operand[3];
		}
	}
}