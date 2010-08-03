using Nexus;
using Rasterizr.VertexAttributes;

namespace Rasterizr.PipelineStages.ShaderStages.VertexShader
{
	public struct VertexShaderOutput
	{
		public Point4D Position;
		public VertexAttribute[] Attributes;
	}
}