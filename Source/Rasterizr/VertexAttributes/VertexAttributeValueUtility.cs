using System;
using Nexus;

namespace Rasterizr.VertexAttributes
{
	public static class VertexAttributeValueUtility
	{
		public static IVertexAttributeValue ToValue(string name, VertexAttributeValueFormat format, object @object)
		{
			// Use Reflection to get value of input element from vertexInput. In the future I'll use Reflection.Emit
			// to create a runtime-generated class to do this more efficiently.
			object value = @object.GetType().GetField(name).GetValue(@object);

			switch (format)
			{
				case VertexAttributeValueFormat.ColorF:
					return new ColorFVertexAttributeValue { Value = (ColorF) value };
				case VertexAttributeValueFormat.Float:
					return new FloatVertexAttributeValue { Value = (float) value };
				case VertexAttributeValueFormat.Point2D:
					return new Point2DVertexAttributeValue { Value = (Point2D) value };
				case VertexAttributeValueFormat.Point3D:
					return new Point3DVertexAttributeValue { Value = (Point3D) value };
				case VertexAttributeValueFormat.Point4D:
					return new Point4DVertexAttributeValue { Value = (Point4D) value };
				case VertexAttributeValueFormat.Vector3D:
					return new Vector3DVertexAttributeValue { Value = (Vector3D) value };
				default:
					throw new NotSupportedException();
			}
		}

		public static IVertexAttributeValue ToValue(string name, Type type, object @object)
		{
			return ToValue(name, GetValueFormat(type), @object);
		}

		private static VertexAttributeValueFormat GetValueFormat(Type type)
		{
			if (type == typeof(float))
				return VertexAttributeValueFormat.Float;
			if (type == typeof(ColorF))
				return VertexAttributeValueFormat.ColorF;
			if (type == typeof(Point2D))
				return VertexAttributeValueFormat.Point2D;
			if (type == typeof(Point3D))
				return VertexAttributeValueFormat.Point3D;
			if (type == typeof(Point4D))
				return VertexAttributeValueFormat.Point4D;
			if (type == typeof(Vector3D))
				return VertexAttributeValueFormat.Vector3D;
			throw new NotSupportedException();
		}
	}
}