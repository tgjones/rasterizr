using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;

namespace Rasterizr.ShaderStages.Core
{
	/// <summary>
	/// Stores metadata for a partiular shader input or output class.
	/// Caches the list of properties, their semantics, etc.
	/// </summary>
	public class ShaderInputOutputDescription
	{
		private readonly IEnumerable<ShaderInputOutputProperty> _properties;

		public IEnumerable<ShaderInputOutputProperty> Properties
		{
			get { return _properties; }
		}

		public ShaderInputOutputDescription(Type type)
		{
			_properties = type.FieldsAndProperties(Flags.Instance | Flags.Public)
				.Where(mi => mi.HasAttribute<SemanticAttribute>())
				.Select(mi =>
				{
					var semanticAttribute = mi.Attribute<SemanticAttribute>();
					return new ShaderInputOutputProperty
					{
						Semantic = new Semantic(semanticAttribute.Name, semanticAttribute.Index),
						MemberInfo = mi
					};
				});
		}

		public object GetValue(object instance, Semantic semantic)
		{
			return FindProperty(semantic).MemberInfo.Get(instance.WrapIfValueType());
		}

		public void SetValue(ref object instance, Semantic semantic, object value)
		{
			var wrapped = instance.WrapIfValueType();
			FindProperty(semantic).MemberInfo.Set(wrapped, value);
			instance = wrapped.UnwrapIfWrapped();
		}

		private ShaderInputOutputProperty FindProperty(Semantic semantic)
		{
			var property = _properties.SingleOrDefault(p => p.Semantic == semantic);
			if (property == null)
				throw new ArgumentException("Could not find property matching semantic", "semantic");
			return property;
		}
	}

	public class ShaderInputOutputProperty
	{
		public MemberInfo MemberInfo { get; set; }
		public ShaderInputOutputPropertyType PropertyType { get; set; }
		public Semantic Semantic { get; set; }
	}

	public enum ShaderInputOutputPropertyType
	{
		ColorF,
		Float,
		Point2D,
		Point3D,
		Point4D,
		Vector3D
	}
}