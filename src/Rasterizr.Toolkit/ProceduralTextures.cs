using Rasterizr.Resources;
using SlimShader;

namespace Rasterizr.Toolkit
{
	public static class ProceduralTextures
	{
		public static Texture2D CreateCheckerboard(Device device, int width, int height)
		{
			var texture = device.CreateTexture2D(new Texture2DDescription
			{
				Width = width,
				Height = height,
				ArraySize = 1
			});
			var data = new Number4[width * height];
			bool black = false;
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					data[(y * width) + x] = (black) ? new Number4(0, 0, 0, 1) : new Number4(1, 1, 1, 1);
					if (x % 8 == 0)
						black = !black;
				}
				if (y % 8 == 0)
					black = !black;
			}
		    texture.SetData(0, data);
			return texture;
		}
	}
}