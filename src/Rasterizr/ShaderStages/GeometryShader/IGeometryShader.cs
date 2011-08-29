using Nexus;
using Rasterizr.Rasterizer;

namespace Rasterizr.ShaderStages.GeometryShader
{
	public interface IGeometryShader
	{
		Color Execute(Fragment fragment);
	}
}