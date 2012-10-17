using System;
using SlimShader.ObjectModel;
using SlimShader.ObjectModel.Tokens;

namespace SlimShader.Parser
{
	public class OperandParser
	{
		private readonly DxbcReader _reader;

		public OperandParser(DxbcReader reader)
		{
			_reader = reader;
		}

		public Operand Parse()
		{
			Operand operand = new Operand();
			operand.IsExtended = Decoder.DecodeIsOpcodeExtended(_reader.CurrentToken);

			operand.IndexDimension = Decoder.DecodeOperandIndexDimension(_reader.CurrentToken);
			operand.OperandType = Decoder.DecodeOperandType(_reader.CurrentToken);

			if (operand.OperandType == OperandType.OPERAND_TYPE_SAMPLER
				|| operand.OperandType == OperandType.OPERAND_TYPE_RESOURCE)
				operand.WriteMaskEnabled = false;

			operand.RegisterNumber = 0;

			var numComponents = Decoder.DecodeOperandNumComponents(_reader.CurrentToken);
			switch (numComponents)
			{
				case OperandNumComponents.OPERAND_0_COMPONENT:
					operand.NumComponents = 0;
					break;
				case OperandNumComponents.OPERAND_1_COMPONENT:
					operand.NumComponents = 1;
					operand.Swizzles[1] = operand.Swizzles[2] = operand.Swizzles[3] = 0;
					break;
				case OperandNumComponents.OPERAND_4_COMPONENT:
					operand.NumComponents = 4;
					operand.SelectionMode = Decoder.DecodeOperand4ComponentSelectionMode(_reader.CurrentToken);

					switch (operand.SelectionMode)
					{
						case Operand4ComponentSelectionMode.OPERAND_4_COMPONENT_MASK_MODE:
							operand.ComponentMask = Decoder.DecodeOperand4ComponentMask(_reader.CurrentToken);
							break;
						case Operand4ComponentSelectionMode.OPERAND_4_COMPONENT_SWIZZLE_MODE:
							operand.Swizzle = Decoder.DecodeOperand4ComponentSwizzle(_reader.CurrentToken);
							//if (operand.Swizzle != Operand4ComponentSwizzles.NoSwizzle)
							//{
								operand.Swizzles[0] = Decoder.DecodeOperand4ComponentSwizzleSource(_reader.CurrentToken, 0);
								operand.Swizzles[1] = Decoder.DecodeOperand4ComponentSwizzleSource(_reader.CurrentToken, 1);
								operand.Swizzles[2] = Decoder.DecodeOperand4ComponentSwizzleSource(_reader.CurrentToken, 2);
								operand.Swizzles[3] = Decoder.DecodeOperand4ComponentSwizzleSource(_reader.CurrentToken, 3);
							//}
							break;
						case Operand4ComponentSelectionMode.OPERAND_4_COMPONENT_SELECT_1_MODE:
							var swizzle = Decoder.DecodeOperand4ComponentSelect1(_reader.CurrentToken);
							operand.Swizzles[0] = operand.Swizzles[1] = operand.Swizzles[2] = operand.Swizzles[3] = swizzle;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					break;
				case OperandNumComponents.OPERAND_N_COMPONENT:
					throw new NotSupportedException();
				default:
					throw new ArgumentOutOfRangeException();
			}

			for (byte i = 0; i < (byte) operand.IndexDimension; i++)
				operand.IndexRepresentations[i] = Decoder.DecodeOperandIndexRepresentation(i, _reader.CurrentToken);

			_reader.MoveNext();

			if (operand.IsExtended)
			{
				//OperandToken1 is the second token.

				operand.Modifier = OperandModifier.OPERAND_MODIFIER_NONE;
				uint operandToken1 = _reader.ReadAndMoveNext();
				if (Decoder.DecodeExtendedOperandType(operandToken1) == ExtendedOperandType.D3D10_SB_EXTENDED_OPERAND_MODIFIER)
					operand.Modifier = Decoder.DecodeExtendedOperandModifier(operandToken1);
			}

			for (byte i = 0; i < (byte)operand.IndexDimension; i++)
			{
				switch (operand.IndexRepresentations[i])
				{
					case OperandIndexRepresentation.OPERAND_INDEX_IMMEDIATE32:
						operand.ArraySizes[i] = _reader.ReadAndMoveNext();
						break;
					case OperandIndexRepresentation.OPERAND_INDEX_IMMEDIATE64:
						goto default;
					case OperandIndexRepresentation.OPERAND_INDEX_RELATIVE:
						operand.Suboperands[i] = Parse();
						break;
					case OperandIndexRepresentation.OPERAND_INDEX_IMMEDIATE32_PLUS_RELATIVE:
						goto default;
					case OperandIndexRepresentation.OPERAND_INDEX_IMMEDIATE64_PLUS_RELATIVE:
						goto default;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			if (operand.OperandType == OperandType.OPERAND_TYPE_IMMEDIATE32)
			{
				for (int i = 0; i < operand.NumComponents; i++)
				{
					byte[] bytes = BitConverter.GetBytes(_reader.ReadAndMoveNext());
					operand.ImmediateValues[i] = BitConverter.ToSingle(bytes, 0);
				}
			}
			else if (operand.OperandType == OperandType.OPERAND_TYPE_IMMEDIATE64)
			{
				throw new NotSupportedException();
			}

			return operand;
		}
	}
}