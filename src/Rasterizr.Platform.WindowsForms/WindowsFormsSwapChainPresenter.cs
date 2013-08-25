using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using SlimShader;
using Point = System.Drawing.Point;

namespace Rasterizr.Platform.WindowsForms
{
	public class WindowsFormsSwapChainPresenter : ISwapChainPresenter
	{
		private readonly int _width, _height;
		private readonly Bitmap _bitmap;
		private readonly Graphics _graphics;

		public WindowsFormsSwapChainPresenter(Form window)
		{
			_width = window.ClientSize.Width;
			_height = window.ClientSize.Height;
			_bitmap = new Bitmap(_width, _height, PixelFormat.Format24bppRgb);
			_graphics = window.CreateGraphics();
		}

	    void ISwapChainPresenter.Initialize(int width, int height) { }

		void ISwapChainPresenter.Present(Number4[] colors)
		{
            // TODO: Convert Color4F[] to byte[] in R8G8B8A8 format.
			var bitmapData = _bitmap.LockBits(new Rectangle(0, 0, _width, _height),
				ImageLockMode.WriteOnly, _bitmap.PixelFormat);

			//Marshal.Copy(colors, 0, bitmapData.Scan0, _width * _height * 3);

			_bitmap.UnlockBits(bitmapData);

			_graphics.DrawImage(_bitmap, Point.Empty);
		}
	}
}