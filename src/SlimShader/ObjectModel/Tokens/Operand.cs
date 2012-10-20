using System;

namespace SlimShader.ObjectModel.Tokens
{
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
		public bool AreImmediateValuesIntegral { get; internal set; }
		public double[] ImmediateValues { get; private set; }

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
			ImmediateValues = new double[4];
		}

		public override string ToString()
		{
			switch (OperandType)
			{
				case OperandType.Immediate32:
				{
					string result = "l(";
					for (int i = 0; i < NumComponents; i++)
					{
						if (AreImmediateValuesIntegral)
						{
							// Just guessing this number based on fxc output.
							const int hexThreshold = 10000;
							bool isHexNumber = ImmediateValues[i] > hexThreshold;
							string formatSpecifier = isHexNumber ? "x8" : "g";
							if (isHexNumber)
								result += "0x";
							result += string.Format("{0:" + formatSpecifier + "}", (long) ImmediateValues[i]);
						}
						else
						{
							result += string.Format("{0:F6}", ImmediateValues[i]);
						}
						if (i < NumComponents - 1)
							result += ", ";
					}
					result += ")";
					return result;
				}
				case OperandType.Null:
				{
					return OperandType.GetDescription();
				}
				default:
				{
					string index = string.Empty;
					switch (IndexDimension)
					{
						case OperandIndexDimension._0D:
							break;
						case OperandIndexDimension._1D:
							index = Indices[0].ToString();
							break;
						case OperandIndexDimension._2D :
							index = string.Format("{0}[{1}]", Indices[0], Indices[1]);
							break;
						case OperandIndexDimension._3D:
							break;
					}

					string components;
					switch (SelectionMode)
					{
						case Operand4ComponentSelectionMode.Mask:
							components = ComponentMask.GetDescription();
							break;
						case Operand4ComponentSelectionMode.Swizzle:
							components = Swizzles[0].GetDescription()
								+ Swizzles[1].GetDescription()
								+ Swizzles[2].GetDescription()
								+ Swizzles[3].GetDescription();
							break;
						case Operand4ComponentSelectionMode.Select1:
							components = Swizzles[0].GetDescription();
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					if (!string.IsNullOrEmpty(components))
						components = "." + components;

					return string.Format("{0}{1}{2}{3}", Modifier.GetDescription(), OperandType.GetDescription(),
						index, components);
				}
			}
		}
	}
}