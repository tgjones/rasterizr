using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Rasterizr.Resources;

namespace Rasterizr.Platform.Wpf
{
	public static class TextureLoader
	{
		public static Texture2D CreateTextureFromStream(Device device, Stream stream)
		{
		    var bitmapFrame = BitmapFrame.Create(stream);
            var bitmap = new FormatConvertedBitmap(bitmapFrame, PixelFormats.Bgra32, null, 0.0);

            var pixelData = new byte[bitmap.PixelWidth * bitmap.PixelHeight * bitmap.Format.BitsPerPixel / 8];
            bitmap.CopyPixels(pixelData, bitmap.PixelWidth * bitmap.Format.BitsPerPixel / 8, 0);

            var colors = new Color4[bitmap.PixelWidth * bitmap.PixelHeight];
		    for (int i = 0; i < colors.Length; i++)
		    {
                var b = pixelData[(i * 4) + 0];
                var g = pixelData[(i * 4) + 1];
                var r = pixelData[(i * 4) + 2];
                var a = pixelData[(i * 4) + 3];

                colors[i] = new Color4(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
		    }

		    return Texture2D.FromColors(device, colors, bitmap.PixelWidth, bitmap.PixelHeight);
		}

		public static WriteableBitmap CreateBitmapFromTexture(Texture2D texture, int arraySlice, int mipLevel)
		{
		    var subresource = Resource.CalculateSubresource(mipLevel, arraySlice, texture.Description.MipLevels);
            var colors = texture.GetData(subresource);

            var pixelData = new byte[colors.Length * 4];
            for (int i = 0; i < colors.Length; i++)
            {
                pixelData[(i * 4) + 0] = (byte) (colors[i].B * 255.0f);
                pixelData[(i * 4) + 1] = (byte) (colors[i].G * 255.0f);
                pixelData[(i * 4) + 2] = (byte) (colors[i].R * 255.0f);
                pixelData[(i * 4) + 3] = (byte) (colors[i].A * 255.0f);
            }

            int width, height;
            texture.GetDimensions(mipLevel, out width, out height);

            var bitmapSouce = BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null,
                pixelData, width * PixelFormats.Bgra32.BitsPerPixel / 8);

            return new WriteableBitmap(bitmapSouce);
		}
	}
}