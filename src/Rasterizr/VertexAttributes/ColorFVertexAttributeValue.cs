using Nexus;

namespace Rasterizr.VertexAttributes
{
	public struct ColorFVertexAttributeValue : IVertexAttributeValue
	{
		public ColorF Value;

		object IVertexAttributeValue.Value
		{
			get { return Value; }
		}

		public IVertexAttributeValue Add(IVertexAttributeValue value)
		{
			return new ColorFVertexAttributeValue
			{
				Value = Value + ((ColorFVertexAttributeValue) value).Value
			};
		}

		public IVertexAttributeValue Multiply(float f)
		{
			return new ColorFVertexAttributeValue
			{
				Value = Value * f
			};
		}

		public IVertexAttributeValue Subtract(IVertexAttributeValue value)
		{
			return new ColorFVertexAttributeValue
			{
				Value = Value - ((ColorFVertexAttributeValue) value).Value
			};
		}
	}
}