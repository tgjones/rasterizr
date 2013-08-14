using Nexus;

namespace Rasterizr.Toolkit.Effects
{
	public class DirectionalLight
	{
		public Color3F DiffuseColor { get; set; }
		public Vector3D Direction { get; set; }
		public bool Enabled { get; set; }
		public Color3F SpecularColor { get; set; }
	}
}