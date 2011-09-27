using System;

namespace Rasterizr.Core.ShaderCore
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class InterpolationModifierAttribute : Attribute
	{
		public InterpolationModifier InterpolationModifier { get; set; }

		public InterpolationModifierAttribute(InterpolationModifier interpolationModifier)
		 {
		 	InterpolationModifier = interpolationModifier;
		 }
	}
}