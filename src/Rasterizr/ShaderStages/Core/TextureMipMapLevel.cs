using Nexus;

namespace Rasterizr.ShaderStages.Core
{
	public class TextureMipMapLevel
	{
		public ColorF[,] Texels { get; private set; }

		public int Level { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }

		public TextureMipMapLevel(int level, int width, int height)
		{
			Level = level;
			Width = width;
			Height = height;

			Texels = new ColorF[width, height];
		}
	}
}