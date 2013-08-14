using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Rasterizr.Resources;
using SlimShader;

namespace Rasterizr.Platform.Wpf
{
	public static class TextureLoader
	{
		public static Texture2D CreateTextureFromStream(Device device, Stream stream)
		{
			var bitmap = new BitmapImage
			{
				CacheOption = BitmapCacheOption.OnLoad
			};
			bitmap.BeginInit();
			bitmap.StreamSource = stream;
			bitmap.EndInit();

			var pixelData = new byte[bitmap.PixelWidth * bitmap.PixelHeight * bitmap.Format.BitsPerPixel / 8];
			bitmap.CopyPixels(pixelData, bitmap.PixelWidth * bitmap.Format.BitsPerPixel / 8, 0);

			var result = device.CreateTexture2D(new Texture2DDescription
			{
				Width = bitmap.PixelWidth,
				Height = bitmap.PixelHeight,
				MipLevels = 0,
				ArraySize = 1,
			});

		    var colors = new Number4[bitmap.PixelWidth * bitmap.PixelHeight];
		    for (int i = 0; i < colors.Length; i++)
		    {
                var b = pixelData[(i * 4) + 0];
                var g = pixelData[(i * 4) + 1];
                var r = pixelData[(i * 4) + 2];
                var a = pixelData[(i * 4) + 3];

                colors[i] = new Number4(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
		    }

		    result.SetData(0, colors);
			device.ImmediateContext.GenerateMips(device.CreateShaderResourceView(result));

			return result;
		}

		public static WriteableBitmap CreateBitmapFromTexture(Texture2D texture, int mipLevel)
		{
		    var subresource = Resource.CalculateSubresource(mipLevel, 0, texture.Description.MipLevels);
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