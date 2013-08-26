using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using SlimShader;

namespace Rasterizr.Platform.Wpf
{
	public class WpfSwapChainPresenter : ISwapChainPresenter
	{
	    private readonly Dispatcher _dispatcher;
	    private int _width;
		private WriteableBitmap _bitmap;
	    private byte[] _outputBytes;

		public WriteableBitmap Bitmap
		{
			get { return _bitmap; }
		}

	    public WpfSwapChainPresenter(Dispatcher dispatcher = null)
	    {
	        _dispatcher = dispatcher;
	    }

	    void ISwapChainPresenter.Initialize(int width, int height)
		{
		    _width = width;

            //_dispatcher.CheckAccess()
	        Action createBitmap = () => _bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
	        if (_dispatcher != null)
	            _dispatcher.Invoke(createBitmap);
	        else
	            createBitmap();

	        _outputBytes = new byte[width * height * 4];
		}

		void ISwapChainPresenter.Present(Number4[] colors)
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
				_bitmap.WritePixels(
                    new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight),
                    _outputBytes, _width * _bitmap.Format.BitsPerPixel / 8, 0);
				_bitmap.Unlock();
			}));
		}
	}
}