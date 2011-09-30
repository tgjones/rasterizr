using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;
using Rasterizr.Core.ShaderCore;

namespace Rasterizr.Core.InputAssembler
{
	public class InputLayoutDescription
	{
		private readonly Dictionary<Semantic, MemberGetter> _cachedGetters; 

		public InputLayoutDescription(InputLayout inputLayout, Type vertexType)
		{
			var vertexFieldsAndProperties = vertexType.FieldsAndProperties()
				.OrderBy(mi => mi.MetadataToken)
				.ToList();

			_cachedGetters = new Dictionary<Semantic, MemberGetter>();
			for (int i = 0; i < inputLayout.Elements.Length; i++)
			{
				var element = inputLayout.Elements[i];
				var fieldOrProperty = vertexFieldsAndProperties[i];
				var getter = (fieldOrProperty.MemberType == MemberTypes.Property)
					? ((PropertyInfo) fieldOrProperty).DelegateForGetPropertyValue()
					: ((FieldInfo) fieldOrProperty).DelegateForGetFieldValue();
				var semantic = new Semantic(element.SemanticName, element.SemanticIndex);
				_cachedGetters[semantic] = getter;
			}
		}

		public object GetValue(object inputVertex, Semantic semantic)
		{
			return _cachedGetters[semantic](inputVertex);
		} 
	}
}