using Rasterizr.PipelineStages.TriangleSetup;

namespace Rasterizr.PipelineStages.ShaderStages.PixelShader
{
	public interface IPixelShaderInputBuilder<out TPixelShaderInput>
	{
		TPixelShaderInput Build(InterpolatedVertexAttribute[] attributes);
	}
}