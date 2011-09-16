using System;

namespace Rasterizr.ShaderCore
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class SemanticAttribute : Attribute
	{
		public string Name { get; set; }
		public int Index { get; set; }

		public SemanticAttribute(string name, int index = 0)
		{
			Name = name;
			Index = index;
		}
	}
}