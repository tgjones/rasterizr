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
		private readonly Type _type;
		private readonly IEnumerable<ShaderInputOutputProperty> _properties;

		public IEnumerable<ShaderInputOutputProperty> Properties
		{
			get { return _properties; }
		}

		public ShaderInputOutputDescription(Type type)
		{
			_type = type;

			_properties = type.Properties(Flags.Instance | Flags.Public)
				.Where(pi => pi.HasAttribute<SemanticAttribute>())
				.Select(pi =>
				{
					var semanticAttribute = pi.Attribute<SemanticAttribute>();
					return new ShaderInputOutputProperty
					{
						Semantic = new Semantic(semanticAttribute.Name, semanticAttribute.Index),
						PropertyInfo = pi
					};
				});
		}

		public object GetValue(object instance, Semantic semantic)
		{
			return FindProperty(semantic).PropertyInfo.Get(instance);
		}

		public void SetValue(object instance, Semantic semantic, object value)
		{
			FindProperty(semantic).PropertyInfo.Set(instance, value);
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
		public PropertyInfo PropertyInfo { get; set; }
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