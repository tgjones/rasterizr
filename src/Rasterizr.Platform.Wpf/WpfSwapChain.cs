using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SlimShader;

namespace Rasterizr.Platform.Wpf
{
	public class WpfSwapChain : SwapChain
	{
		private readonly WriteableBitmap _bitmap;
	    private readonly byte[] _outputBytes;

		public WriteableBitmap Bitmap
		{
			get { return _bitmap; }
		}

		public WpfSwapChain(Device device, int width, int height)
			: base(device, new SwapChainDescription
			{
				Width = width,
				Height = height,
				SampleDescription = new SampleDescription(1)
			})
		{
			_bitmap = new WriteableBitmap(Description.Width, Description.Height, 96, 96, PixelFormats.Bgra32, null);
		    _outputBytes = new byte[width * height * 4];
		}

		protected override void Present(Number4[] colors)
		{
            for (var i = 0; i < colors.Length; i++)
            {
                _outputBytes[(i * 4) + 0] = (byte) (colors[i].B * 255.0f);
                _outputBytes[(i * 4) + 1] = (byte) (colors[i].G * 255.0f);
                _outputBytes[(i * 4) + 2] = (byte) (colors[i].R * 255.0f);
                _outputBytes[(i * 4) + 3] = (byte) (colors[i].A * 255.0f);
            }

			_bitmap.Dispatcher.Invoke((Action) (() =>
			{
				_bitmap.Lock();
				_bitmap.WritePixels(new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight),
					_outputBytes, Description.Width * _bitmap.Format.BitsPerPixel / 8, 0);
				_bitmap.Unlock();
			}));
		}
	}
}