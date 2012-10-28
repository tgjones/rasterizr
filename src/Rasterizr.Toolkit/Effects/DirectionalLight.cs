using Nexus;
using Nexus.Graphics.Colors;

namespace Rasterizr.Toolkit.Effects
{
	public class DirectionalLight
	{
		public ColorRgbF DiffuseColor { get; set; }
		public Vector3D Direction { get; set; }
		public bool Enabled { get; set; }
		public ColorRgbF SpecularColor { get; set; }
	}
}