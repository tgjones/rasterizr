using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;

namespace Rasterizr.ShaderCore
{
	public class ShaderDescription
	{
		private readonly Dictionary<Semantic, SignatureParameterDescription> _outputParametersDictionary;
		private readonly MemberGetter[] _textureGetters;

		public SignatureParameterDescription[] InputParameters { get; private set; }
		public SignatureParameterDescription[] OutputParameters { get; private set; }

		public ShaderDescription(IShader shader)
		{
			// Extract input and output parameters.
			InputParameters = FindParameters(shader.InputType);
			OutputParameters = FindParameters(shader.OutputType);

			_outputParametersDictionary = OutputParameters.ToDictionary(spd => spd.Semantic);

			_textureGetters = FindTextureGetters(shader);
		}

		private static MemberGetter[] FindTextureGetters(IShader shader)
		{
			return shader.GetType().Properties()
				.Where(pi => pi.PropertyType == typeof (Texture2D))
				.Select(pi => pi.DelegateForGetPropertyValue())
				.ToArray();
		}

		private SignatureParameterDescription[] FindParameters(Type type)
		{
			return type.FieldsAndProperties(Flags.Instance | Flags.Public)
				.Where(mi => mi.HasAttribute<SemanticAttribute>())
				.Select(mi =>
				{
					var semanticAttribute = mi.Attribute<SemanticAttribute>();
					var interpolationModifierAttribute = mi.Attribute<InterpolationModifierAttribute>();
					var interpolationModifier = (interpolationModifierAttribute != null)
						? interpolationModifierAttribute.InterpolationModifier
						: InterpolationModifier.PerspectiveCorrect;
					return new SignatureParameterDescription
					{
						Semantic = semanticAttribute.Semantic,
						InterpolationModifier = interpolationModifier,
						ParameterType = (mi.MemberType == MemberTypes.Property)
							? ((PropertyInfo)mi).PropertyType
							: ((FieldInfo)mi).FieldType,
						Getter = (mi.MemberType == MemberTypes.Property)
							? ((PropertyInfo) mi).DelegateForGetPropertyValue()
							: ((FieldInfo) mi).DelegateForGetFieldValue(),
						Setter = (mi.MemberType == MemberTypes.Property)
							? ((PropertyInfo) mi).DelegateForSetPropertyValue()
							: ((FieldInfo) mi).DelegateForSetFieldValue()
					};
				}).ToArray();
		}

		public SignatureParameterDescription GetOutputParameterBySemantic(Semantic semantic)
		{
			SignatureParameterDescription result;
			_outputParametersDictionary.TryGetValue(semantic, out result);
			return result;
		}

		public IList<Texture2D> GetTextureParameters(IShader shader)
		{
			return _textureGetters
				.Select(mg => (Texture2D) mg(shader))
				.ToList();
		}
	}
}