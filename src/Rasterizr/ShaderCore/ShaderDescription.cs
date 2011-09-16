using System;
using System.Collections.Generic;
using System.Linq;
using Fasterflect;

namespace Rasterizr.ShaderCore
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
					return new SignatureParameterDescription
					{
						Semantic = new Semantic(semanticAttribute.Name, semanticAttribute.Index),
						MemberInfo = mi
					};
				}).ToArray();
		}

		public SignatureParameterDescription GetOutputParameterBySemantic(Semantic semantic)
		{
			return _outputParametersDictionary[semantic];
		}
	}
}