using System.Reflection;
using Fasterflect;

namespace Rasterizr.ShaderCore
{
	public class SignatureParameterDescription
	{
		public Semantic Semantic { get; set; }
		public SystemValueType SystemValueType { get; set; }
		public MemberInfo MemberInfo { get; set; }

		public object GetValue(object instance)
		{
			return MemberInfo.Get(instance.WrapIfValueType());
		}

		public void SetValue(ref object instance, object value)
		{
			var wrapped = instance.WrapIfValueType();
			MemberInfo.Set(wrapped, value);
			instance = wrapped.UnwrapIfWrapped();
		}
	}
}