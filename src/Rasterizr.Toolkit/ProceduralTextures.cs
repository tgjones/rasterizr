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
				ArraySize = 1,
				Format = Format.R8G8B8A8_UInt
			});
			var data = new Color4[width * height];
			bool black = false;
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					data[(y * width) + x] = (black) ? Color4.Black : Color4.White;
					black = !black;
				}
				black = !black;
			}
			device.ImmediateContext.UpdateSubresource(texture, 0, data);
			return texture;
		}
	}
}