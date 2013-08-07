using Rasterizr.Math;
using Rasterizr.Resources;

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
			var data = new Color4F[width * height];
			bool black = false;
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					data[(y * width) + x] = (black) ? Color4F.Black : Color4F.White;
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