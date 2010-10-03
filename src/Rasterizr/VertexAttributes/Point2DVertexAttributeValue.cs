using Nexus;

namespace Rasterizr.VertexAttributes
{
	public struct Point2DVertexAttributeValue : IVertexAttributeValue
	{
		public Point2D Value;

		object IVertexAttributeValue.Value
		{
			get { return Value; }
		}

		public IVertexAttributeValue Add(IVertexAttributeValue value)
		{
			return new Point2DVertexAttributeValue
			{
				Value = Value + ((Point2DVertexAttributeValue) value).Value
			};
		}

		public IVertexAttributeValue Multiply(float f)
		{
			return new Point2DVertexAttributeValue
			{
				Value = Value * f
			};
		}

		public IVertexAttributeValue Subtract(IVertexAttributeValue value)
		{
			return new Point2DVertexAttributeValue
			{
				Value = (Point2D) (Value - ((Point2DVertexAttributeValue) value).Value)
			};
		}
	}
}