using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Rasterizr.Util
{
	/// <summary>
	/// With thanks to http://writeablebitmapex.codeplex.com/
	/// </summary>
	public class WriteableBitmapWrapper
	{
		#region Fields

		private const float PreMultiplyFactor = 1 / 255f;
		private const int SizeOfARGB = 4;

		private WriteableBitmap _inner;
#if !SILVERLIGHT
		private int[] _pixels;
#endif

		#endregion

		#region Properties

#if !SILVERLIGHT
		public int[] Pixels
		{
			get { return _pixels; }
		}
#endif

		public WriteableBitmap InnerBitmap
		{
			get { return _inner; }
		}

		#endregion

		public WriteableBitmapWrapper(BitmapSource source)
		{
			_inner = new WriteableBitmap(source);

#if !SILVERLIGHT
			int width = _inner.PixelWidth;
			int height = _inner.PixelHeight;
			_pixels = new int[width * height];
			_inner.CopyPixels(_pixels, width * 4, 0);
#endif
		}

		private int[] GetPixels()
		{
#if SILVERLIGHT
			return _inner.Pixels;
#else
			return _pixels;
#endif
		}

		public WriteableBitmapWrapper(int width, int height)
		{
#if !SILVERLIGHT
			_inner = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
#else
			_inner = new WriteableBitmap(width, height);
#endif

#if !SILVERLIGHT
			_pixels = new int[width * height];
			_inner.CopyPixels(_pixels, width * 4, 0);
#endif
		}

		#region Clear

		/// <summary>
		/// Fills the whole WriteableBitmap with a color.
		/// </summary>
		/// <param name="bmp">The WriteableBitmap.</param>
		/// <param name="color">The color used for filling.</param>
		public void Clear(Color color)
		{
			float ai = color.A * PreMultiplyFactor;
			int col = (color.A << 24) | ((byte) (color.R * ai) << 16) | ((byte) (color.G * ai) << 8) | (byte) (color.B * ai);
			int[] pixels = GetPixels();
			int w = _inner.PixelWidth;
			int h = _inner.PixelHeight;
			int len = w * SizeOfARGB;

			// Fill first line
			for (int x = 0; x < w; x++)
			{
				pixels[x] = col;
			}

			// Copy first line
			for (int y = 1; y < h; y++)
			{
				Buffer.BlockCopy(pixels, 0, pixels, y * len, len);
			}
		}

		/// <summary>
		/// Fills the whole WriteableBitmap with an empty color (0).
		/// </summary>
		/// <param name="bmp">The WriteableBitmap.</param>
		public void Clear()
		{
			int[] pixels = GetPixels();
			Array.Clear(pixels, 0, pixels.Length);
		}

		#endregion

		#region GetPixel

		/// <summary>
		/// Gets the color of the pixel at the x, y coordinate as a Color struct.
		/// </summary>
		/// <param name="bmp">The WriteableBitmap.</param>
		/// <param name="x">The x coordinate of the pixel.</param>
		/// <param name="y">The y coordinate of the pixel.</param>
		/// <returns>The color of the pixel at x, y as a Color struct.</returns>
		public Color GetPixel(int x, int y)
		{
			int c = GetPixels()[y * _inner.PixelWidth + x];
			byte a = (byte) (c >> 24);
			//float ai = a / PreMultiplyFactor;
			float ai = 1;
			return Color.FromArgb(a, (byte) ((c >> 16) * ai), (byte) ((c >> 8) * ai), (byte) (c * ai));
		}

		#endregion

		#region SetPixel

		/// <summary>
		/// Sets the color of the pixel.
		/// </summary>
		/// <param name="bmp">The WriteableBitmap.</param>
		/// <param name="x">The x coordinate (row).</param>
		/// <param name="y">The y coordinate (column).</param>
		/// <param name="color">The color.</param>
		public void SetPixel(int x, int y, Color color)
		{
			if (x < 0 || y < 0 || x >= _inner.PixelWidth || y >= _inner.PixelHeight)
				return;

			float ai = color.A * PreMultiplyFactor;
			GetPixels()[y * _inner.PixelWidth + x] = (color.A << 24) | ((byte) (color.R * ai) << 16) | ((byte) (color.G * ai) << 8) | (byte) (color.B * ai);
		}

		#endregion
	}
}