using Nexus;
using Rasterizr.PipelineStages.Rasterizer;

namespace Rasterizr.PipelineStages.ShaderStages.GeometryShader
{
	public interface IGeometryShader
	{
		Color Execute(Fragment fragment);
	}
}