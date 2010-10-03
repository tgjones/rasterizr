using Nexus;

namespace Rasterizr.VertexAttributes
{
	public struct Vector3DVertexAttributeValue : IVertexAttributeValue
	{
		public Vector3D Value;

		object IVertexAttributeValue.Value
		{
			get { return Value; }
		}

		public IVertexAttributeValue Add(IVertexAttributeValue value)
		{
			return new Vector3DVertexAttributeValue
			{
				Value = Value + ((Vector3DVertexAttributeValue) value).Value
			};
		}

		public IVertexAttributeValue Multiply(float f)
		{
			return new Vector3DVertexAttributeValue
			{
				Value = Value * f
			};
		}

		public IVertexAttributeValue Subtract(IVertexAttributeValue value)
		{
			return new Vector3DVertexAttributeValue
			{
				Value = Value - ((Vector3DVertexAttributeValue) value).Value
			};
		}
	}
}