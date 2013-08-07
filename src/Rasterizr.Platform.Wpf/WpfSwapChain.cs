using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Rasterizr.Util;
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
                var color = colors[i].ToColor4();

                _outputBytes[(i * 4) + 0] = color.B;
                _outputBytes[(i * 4) + 1] = color.G;
                _outputBytes[(i * 4) + 2] = color.R;
                _outputBytes[(i * 4) + 3] = color.A;
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