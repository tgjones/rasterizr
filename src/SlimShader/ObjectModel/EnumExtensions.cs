using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SlimShader.ObjectModel
{
	internal static class EnumExtensions
	{
		private static readonly Dictionary<Type, Dictionary<Enum, string>> TypeDescriptions;

		static EnumExtensions()
		{
			TypeDescriptions = new Dictionary<Type, Dictionary<Enum, string>>();
		}

		public static string GetDescription(this Enum value)
		{
			Type type = value.GetType();
			if (!TypeDescriptions.ContainsKey(type))
				TypeDescriptions[type] = Enum.GetValues(type).Cast<Enum>().Distinct()
					.ToDictionary(x => x, GetDescriptionInternal);
			return TypeDescriptions[type][value];
		}

		private static string GetDescriptionInternal(Enum value)
		{
			FieldInfo field = value.GetType().GetField(value.ToString());
			var attribute = Attribute.GetCustomAttribute(
				field, typeof(DescriptionAttribute)) as DescriptionAttribute;
			return attribute == null ? value.ToString() : attribute.Description;
		}

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

		public static bool IsIntegralTypeInstruction(this OpcodeType type)
		{
			switch (type)
			{
				case OpcodeType.And :
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
				case OpcodeType.Mov:
				case OpcodeType.UShr :
				case OpcodeType.Xor:
					return true;
				default:
					return false;
			}
		}
	}
}