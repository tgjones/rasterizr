using Nexus;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader.VertexAttributes
{
	public struct Point4DVertexAttributeValue : IVertexAttributeValue
	{
		public Point4D Value;

		object IVertexAttributeValue.Value
		{
			get { return Value; }
		}

		public IVertexAttributeValue Add(IVertexAttributeValue value)
		{
			return new Point4DVertexAttributeValue
			{
				Value = Value + ((Point4DVertexAttributeValue) value).Value
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
			return new Point4DVertexAttributeValue
			{
				Value = Value - ((Point4DVertexAttributeValue) value).Value
			};
		}
	}
}