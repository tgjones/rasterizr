using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Rasterizr.Resources;

namespace Rasterizr.Platform.Wpf
{
	public static class TextureHelper
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
				Format = Format.B8G8R8A8_UInt
			});

			device.ImmediateContext.UpdateSubresource(result, 0, pixelData);
			device.ImmediateContext.GenerateMips(device.CreateShaderResourceView(result));

			return result;
		}

		public static WriteableBitmap CreateBitmapFromTexture(Texture2D texture, int mipLevel)
		{
			var mappedSubresource = texture.Map(mipLevel);

			int width, height;
			texture.GetDimensions(mipLevel, out width, out height);

			var bitmapSouce = BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null,
				mappedSubresource.Data, width * PixelFormats.Bgra32.BitsPerPixel / 8);

			return new WriteableBitmap(bitmapSouce);
		}
	}
}