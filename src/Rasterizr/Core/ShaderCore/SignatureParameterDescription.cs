using Fasterflect;

namespace Rasterizr.Core.ShaderCore
{
	public class SignatureParameterDescription
	{
		public Semantic Semantic { get; set; }
		public SystemValueType SystemValueType { get; set; }
		public InterpolationModifier InterpolationModifier { get; set; }
		public MemberGetter Getter { get; set; }
		public MemberSetter Setter { get; set; }

		public object GetValue(object instance)
		{
			return Getter(instance.WrapIfValueType());
		}

		public void SetValue(ref object instance, object value)
		{
			var wrapped = instance.WrapIfValueType();
			Setter(wrapped, value);
			instance = wrapped.UnwrapIfWrapped();
		}
	}
}