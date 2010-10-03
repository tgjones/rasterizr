using Nexus;

namespace Rasterizr.VertexAttributes
{
	public struct Point3DVertexAttributeValue : IVertexAttributeValue
	{
		public Point3D Value;

		object IVertexAttributeValue.Value
		{
			get { return Value; }
		}

		public IVertexAttributeValue Add(IVertexAttributeValue value)
		{
			return new Point3DVertexAttributeValue
			{
				Value = Value + ((Point3DVertexAttributeValue) value).Value
			};
		}

		public IVertexAttributeValue Multiply(float f)
		{
			throw new System.NotImplementedException();
			/*return new Point4DVertexAttributeValue
			{
				Value = Value * f
			};*/
		}

		public IVertexAttributeValue Subtract(IVertexAttributeValue value)
		{
			return new Point3DVertexAttributeValue
			{
				Value = (Point3D) (Value - ((Point3DVertexAttributeValue) value).Value)
			};
		}
	}
}