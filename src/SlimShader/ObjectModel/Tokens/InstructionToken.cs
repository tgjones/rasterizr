using System.Collections.Generic;

namespace SlimShader.ObjectModel.Tokens
{
	public class InstructionToken : OpcodeToken
	{
		public bool Saturate { get; internal set; }
		public InstructionTestBoolean TestBoolean { get; internal set; }
		public List<InstructionTokenExtendedType> ExtendedTypes { get; private set; }
		public uint[] SampleOffsets { get; private set; }
		public byte ResourceTarget { get; internal set; }
		public byte[] ResourceReturnTypes { get; private set; }
		public List<Operand> Operands { get; private set; }

		public InstructionToken()
		{
			ExtendedTypes = new List<InstructionTokenExtendedType>();
			SampleOffsets = new uint[3];
			ResourceReturnTypes = new byte[4];
			Operands = new List<Operand>();
		}

		public override string ToString()
		{
			string result = TypeDescription;

			if (ExtendedTypes.Contains(InstructionTokenExtendedType.SampleControls))
				result += string.Format("({0},{1},{2})", SampleOffsets[0], SampleOffsets[1], SampleOffsets[2]);

			if (Header.OpcodeType.IsConditionalInstruction())
				result += "_" + TestBoolean.GetDescription();

			if (Saturate)
				result += "_sat";
			result += " ";

			for (int i = 0; i < Operands.Count; i++)
			{
				result += Operands[i].ToString();
				if (i < Operands.Count - 1)
					result += ", ";
			}

			return result;
		}
	}
}