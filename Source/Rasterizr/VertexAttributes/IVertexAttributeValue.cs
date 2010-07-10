namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader.VertexAttributes
{
	public interface IVertexAttributeValue
	{
		object Value { get; }

		IVertexAttributeValue Add(IVertexAttributeValue value);
		IVertexAttributeValue Multiply(float f);
		IVertexAttributeValue Subtract(IVertexAttributeValue value);
	}
}