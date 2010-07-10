using System;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader.VertexAttributes;
using Nexus;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.VertexAttributes
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
				case VertexAttributeValueFormat.Float:
					return new FloatVertexAttributeValue { Value = (float) value };
				case VertexAttributeValueFormat.Point3D:
					return new Point3DVertexAttributeValue { Value = (Point3D) value };
				case VertexAttributeValueFormat.Point4D:
					return new Point4DVertexAttributeValue { Value = (Point4D) value };
				case VertexAttributeValueFormat.ColorF:
					return new ColorFVertexAttributeValue { Value = (ColorF) value };
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
			if (type == typeof(Point4D))
				return VertexAttributeValueFormat.Point4D;
			if (type == typeof(ColorF))
				return VertexAttributeValueFormat.ColorF;
			throw new NotSupportedException();
		}
	}
}