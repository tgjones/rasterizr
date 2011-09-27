using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;

namespace Rasterizr.Core.ShaderCore
{
	public class ShaderDescription
	{
		private readonly Dictionary<Semantic, SignatureParameterDescription> _outputParametersDictionary;

		public SignatureParameterDescription[] InputParameters { get; private set; }
		public SignatureParameterDescription[] OutputParameters { get; private set; }

		public ShaderDescription(IShader shader)
		{
			// Extract input and output parameters.
			InputParameters = FindParameters(shader.InputType);
			OutputParameters = FindParameters(shader.OutputType);

			_outputParametersDictionary = OutputParameters.ToDictionary(spd => spd.Semantic);
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
	}
}