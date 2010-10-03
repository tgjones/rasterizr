using Nexus;

namespace Rasterizr.PipelineStages.ShaderStages.VertexShader
{
	public interface IVertexShaderOutput
	{
		Point4D Position { get; set; }
	}
}