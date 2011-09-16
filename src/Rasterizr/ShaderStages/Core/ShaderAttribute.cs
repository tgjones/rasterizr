using System;

namespace Rasterizr.ShaderStages.Core
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ShaderAttribute : Attribute
	{
		public Type InputType { get; set; }
		public Type OutputType { get; set; }

		public ShaderAttribute(Type inputType, Type outputType)
		{
			InputType = inputType;
			OutputType = outputType;
		}
	}
}