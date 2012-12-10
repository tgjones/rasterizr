using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Rasterizr.Platform.Wpf
{
	public class WpfSwapChain : SwapChain
	{
		private readonly WriteableBitmap _bitmap;

		public WriteableBitmap Bitmap
		{
			get { return _bitmap; }
		}

		public WpfSwapChain(Device device, Image image)
			: this(device, (int) image.Width, (int) image.Height)
		{
			image.Source = _bitmap;
		}

		public WpfSwapChain(Device device, int width, int height)
			: base(device, new SwapChainDescription
			{
				Width = width,
				Height = height,
				Format = Format.R8G8B8A8_UInt,
				SampleDescription = new SampleDescription(1)
			})
		{
			_bitmap = new WriteableBitmap(Description.Width, Description.Height, 96, 96, PixelFormats.Bgra32, null);
		}

		protected override void Present(byte[] colors)
		{
			_bitmap.Dispatcher.Invoke((Action) (() =>
			{
				_bitmap.Lock();
				_bitmap.WritePixels(new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight),
				                    colors, Description.Width*_bitmap.Format.BitsPerPixel/8, 0);
				_bitmap.Unlock();
			}));
		}
	}
}