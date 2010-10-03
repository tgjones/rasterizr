using Nexus;

namespace Rasterizr.PipelineStages.ShaderStages.Core
{
	public class DirectionalLight
	{
		public ColorRgbF DiffuseColor { get; set; }
		public Vector3D Direction { get; set; }
		public bool Enabled { get; set; }
		public ColorRgbF SpecularColor { get; set; }
	}
}