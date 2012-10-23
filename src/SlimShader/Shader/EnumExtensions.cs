using System;

namespace SlimShader.Shader
{
	internal static class EnumExtensions
	{
		public static string GetDescription(this ComponentMask value)
		{
			string result = string.Empty;

			if (value.HasFlag(ComponentMask.X))
				result += "x";
			if (value.HasFlag(ComponentMask.Y))
				result += "y";
			if (value.HasFlag(ComponentMask.Z))
				result += "z";
			if (value.HasFlag(ComponentMask.W))
				result += "w";
			
			return result;
		}

		public static string GetDescription(this Operand4ComponentName value)
		{
			switch (value)
			{
				case Operand4ComponentName.X:
					return "x";
				case Operand4ComponentName.Y:
					return "y";
				case Operand4ComponentName.Z:
					return "z";
				case Operand4ComponentName.W:
					return "w";
				default:
					throw new ArgumentOutOfRangeException("value");
			}
		}

		public static bool IsConditionalInstruction(this OpcodeType type)
		{
			switch (type)
			{
				case OpcodeType.BreakC :
				case OpcodeType.CallC :
				case OpcodeType.If :
					return true;
				default :
					return false;
			}
		}

		public static bool IsDeclaration(this OpcodeType type)
		{
			return (type >= OpcodeType.DclResource && type <= OpcodeType.DclGlobalFlags)
				|| (type >= OpcodeType.DclStream && type <= OpcodeType.DclResourceStructured)
				|| type == OpcodeType.DclGsInstanceCount;
		}

		public static bool IsIntegralTypeInstruction(this OpcodeType type)
		{
			switch (type)
			{
				case OpcodeType.And :
				case OpcodeType.Case :
				case OpcodeType.IAdd:
				case OpcodeType.IBfe:
				case OpcodeType.IEq:
				case OpcodeType.IGe:
				case OpcodeType.ILt:
				case OpcodeType.IMad:
				case OpcodeType.IMax:
				case OpcodeType.IMin:
				case OpcodeType.IMul:
				case OpcodeType.INe:
				case OpcodeType.INeg:
				case OpcodeType.IShl:
				case OpcodeType.IShr:
				case OpcodeType.LdMs:
				case OpcodeType.UShr :
				case OpcodeType.Xor:
					return true;
				default:
					return false;
			}
		}

		public static bool IsDoubleTypeInstruction(this OpcodeType type)
		{
			switch (type)
			{
				case OpcodeType.DAdd:
				case OpcodeType.DMax:
				case OpcodeType.DMin:
				case OpcodeType.DMul:
				case OpcodeType.DEq:
				case OpcodeType.DGe:
				case OpcodeType.DLt:
				case OpcodeType.DNe:
				case OpcodeType.DMov:
				case OpcodeType.DMovC:
					return true;
				default:
					return false;
			}
		}

		public static string Wrap(this OperandModifier modifier, string valueToWrap)
		{
			switch (modifier)
			{
				case OperandModifier.None:
					return valueToWrap;
				case OperandModifier.Neg:
					return "-" + valueToWrap;
				case OperandModifier.Abs:
					return "|" + valueToWrap + "|";
				case OperandModifier.AbsNeg:
					return "-|" + valueToWrap + "|";
				default:
					throw new ArgumentOutOfRangeException("modifier");
			}
		}

		public static bool IsNestedSectionStart(this OpcodeType type)
		{
			switch (type)
			{
				case OpcodeType.Loop :
				case OpcodeType.If :
				case OpcodeType.Else :
				case OpcodeType.Switch :
					return true;
				default :
					return false;
			}
		}

		public static bool IsNestedSectionEnd(this OpcodeType type)
		{
			switch (type)
			{
				case OpcodeType.EndLoop:
				case OpcodeType.EndIf:
				case OpcodeType.Else:
				case OpcodeType.EndSwitch:
					return true;
				default:
					return false;
			}
		}
	}
}