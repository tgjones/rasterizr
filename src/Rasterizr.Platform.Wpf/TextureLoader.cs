using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Rasterizr.Math;
using Rasterizr.Resources;

namespace Rasterizr.Platform.Wpf
{
	public static class TextureLoader
	{
		public static Texture2D CreateTextureFromFile(Device device, Stream stream)
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

		    var colors = new Color4F[bitmap.PixelWidth * bitmap.PixelHeight];
		    for (int i = 0; i < colors.Length; i++)
		    {
                var b = pixelData[(i * 4) + 0];
                var g = pixelData[(i * 4) + 1];
                var r = pixelData[(i * 4) + 2];
                var a = pixelData[(i * 4) + 3];

                colors[i] = new Color4(r, g, b, a).ToColor4F();
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
                var color = colors[i].ToColor4();

                pixelData[(i * 4) + 0] = color.B;
                pixelData[(i * 4) + 1] = color.G;
                pixelData[(i * 4) + 2] = color.R;
                pixelData[(i * 4) + 3] = color.A;
            }

            int width, height;
            texture.GetDimensions(mipLevel, out width, out height);

            var bitmapSouce = BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null,
                pixelData, width * PixelFormats.Bgra32.BitsPerPixel / 8);

            return new WriteableBitmap(bitmapSouce);
		}
	}
}