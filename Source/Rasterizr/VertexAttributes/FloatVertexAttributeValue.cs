namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader.VertexAttributes
{
	public struct FloatVertexAttributeValue : IVertexAttributeValue
	{
		public float Value;

		object IVertexAttributeValue.Value
		{
			get { return Value; }
		}

		public IVertexAttributeValue Add(IVertexAttributeValue value)
		{
			return new FloatVertexAttributeValue
			{
				Value = Value + ((FloatVertexAttributeValue) value).Value
			};
		}

		public IVertexAttributeValue Multiply(float f)
		{
			return new FloatVertexAttributeValue
			{
				Value = Value * f
			};
		}

		public IVertexAttributeValue Subtract(IVertexAttributeValue value)
		{
			return new FloatVertexAttributeValue
			{
				Value = Value - ((FloatVertexAttributeValue) value).Value
			};
		}
	}
}