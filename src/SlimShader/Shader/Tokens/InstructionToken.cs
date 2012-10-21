using System;
using System.Collections.Generic;
using SlimShader.IO;
using SlimShader.Util;

namespace SlimShader.Shader.Tokens
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

		public static InstructionToken Parse(BytecodeReader reader, OpcodeHeader header, uint token0)
		{
			var instructionToken = new InstructionToken();

			instructionToken.Saturate = (token0.DecodeValue(13, 13) == 1);
			instructionToken.TestBoolean = token0.DecodeValue<InstructionTestBoolean>(18, 18);

			// Advance to next token.
			var instructionEnd = reader.CurrentPosition + (header.Length * sizeof(uint));
			reader.ReadUInt32();

			bool extended = header.IsExtended;
			while (extended)
			{
				uint extendedToken = reader.ReadUInt32();
				var extendedType = extendedToken.DecodeValue<InstructionTokenExtendedType>(0, 6);
				instructionToken.ExtendedTypes.Add(extendedType);
				extended = (extendedToken.DecodeValue(31, 31) == 1);

				switch (extendedType)
				{
					case InstructionTokenExtendedType.SampleControls:
						instructionToken.SampleOffsets[0] = extendedToken.DecodeValue(09, 12);
						instructionToken.SampleOffsets[1] = extendedToken.DecodeValue(13, 16);
						instructionToken.SampleOffsets[2] = extendedToken.DecodeValue(17, 20);
						break;
					case InstructionTokenExtendedType.ResourceDim:
						instructionToken.ResourceTarget = extendedToken.DecodeValue<byte>(6, 10);
						break;
					case InstructionTokenExtendedType.ResourceReturnType:
						instructionToken.ResourceReturnTypes[0] = extendedToken.DecodeValue<byte>(06, 09);
						instructionToken.ResourceReturnTypes[1] = extendedToken.DecodeValue<byte>(10, 12);
						instructionToken.ResourceReturnTypes[2] = extendedToken.DecodeValue<byte>(13, 16);
						instructionToken.ResourceReturnTypes[3] = extendedToken.DecodeValue<byte>(17, 20);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			if (header.OpcodeType == OpcodeType.InterfaceCall)
			{
				// TODO
				reader.ReadUInt32();
			}

			while (reader.CurrentPosition < instructionEnd)
				instructionToken.Operands.Add(Operand.Parse(reader, header.OpcodeType));

			return instructionToken;
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