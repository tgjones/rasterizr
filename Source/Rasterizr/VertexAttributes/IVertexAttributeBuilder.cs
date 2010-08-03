namespace Rasterizr.VertexAttributes
{
	public interface IVertexAttributeBuilder<in TVertexShaderOutput>
	{
		VertexAttribute[] Build(TVertexShaderOutput vertexShaderOutput);
	}
}