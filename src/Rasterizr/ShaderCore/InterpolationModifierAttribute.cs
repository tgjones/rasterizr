using System;

namespace Rasterizr.ShaderCore
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