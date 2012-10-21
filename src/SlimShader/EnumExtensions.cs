using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SlimShader
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
	}
}