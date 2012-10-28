using System;

namespace Rasterizr.ShaderCore
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class SemanticAttribute : Attribute
	{
		public string Name { get; set; }
		public int Index { get; set; }
		public SystemValueType SystemValue { get; set; }

		internal Semantic Semantic
		{
			get
			{
				if (SystemValue == SystemValueType.None)
					return new Semantic(Name, Index);
				return new Semantic(SystemValue);
			}
		}

		public SemanticAttribute(string name, int index = 0)
		{
			Name = name;
			Index = index;
		}

		public SemanticAttribute(SystemValueType systemValue)
		{
			SystemValue = systemValue;
		}
	}
}