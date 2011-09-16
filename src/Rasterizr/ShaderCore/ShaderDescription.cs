using System;
using System.Linq;
using Fasterflect;

namespace Rasterizr.ShaderCore
{
	public class ShaderDescription
	{
		public SignatureParameterDescription[] InputParameters { get; private set; }
		public SignatureParameterDescription[] OutputParameters { get; private set; }

		public ShaderDescription(Type shaderType)
		{
			// Check that type has shader attribute.
			if (!shaderType.HasAttribute<ShaderAttribute>())
				throw new ArgumentException("Shader must have a Shader attribute.");

			// Extract input and output parameters.
			var shaderAttribute = shaderType.Attribute<ShaderAttribute>();
			InputParameters = FindParameters(shaderAttribute.InputType);
			OutputParameters = FindParameters(shaderAttribute.OutputType);
		}

		private SignatureParameterDescription[] FindParameters(Type type)
		{
			return type.FieldsAndProperties(Flags.Instance | Flags.Public)
				.Where(mi => AttributeExtensions.HasAttribute<SemanticAttribute>(mi))
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
			throw new NotImplementedException();
		}

		/// <summary>
		/// Checks that the output of this shader is compatible with the
		/// input of the specified shader.
		/// </summary>
		/// <param name="downstream"></param>
		/// <returns></returns>
		public bool IsCompatibleWith(ShaderDescription downstream)
		{
			// TODO: Check that downstream only contains elements either:
			// (a) present in this object, or
			// (b) system values.
			throw new NotImplementedException();
		}


		//public void Populate(object instance, object upstreamInstance, ShaderDescription upstreamDescription)
		//{
		//    foreach (var inputParameter in InputParameters)
		//    {
		//        if (inputParameter.SystemValueType == SystemValueType.None)
		//        {
		//            var outputParameter = upstreamDescription.GetOutputParameterBySemantic(inputParameter.Semantic);
		//            var outputParameterValue = outputParameter.GetValue(upstreamInstance);
		//            inputParameter.SetValue(instance, outputParameterValue);
		//        }
		//        else
		//        {
		//            // Set system value.
		//        }
		//    }
		//}
	}
}