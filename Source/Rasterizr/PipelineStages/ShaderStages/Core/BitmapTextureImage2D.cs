using System;
using System.Windows.Media.Imaging;
using Nexus;
using Rasterizr.Util;

namespace Rasterizr.PipelineStages.ShaderStages.Core
{
	public class BitmapTextureImage2D : ITextureImage
	{
		private readonly WriteableBitmapWrapper _wrapper;

		public BitmapTextureImage2D(BitmapSource bitmapSource)
		{
			_wrapper = new WriteableBitmapWrapper(bitmapSource);
		}

		public int Dimensions
		{
			get { return 2; }
		}

		public int GetBound(int dimension)
		{
			switch (dimension)
			{
				case 0 :
					return _wrapper.InnerBitmap.PixelWidth;
				case 1 :
					return _wrapper.InnerBitmap.PixelHeight;
				default :
					throw new ArgumentOutOfRangeException();
			}
		}

		public ColorF GetColor(int x, int y)
		{
			System.Windows.Media.Color c = _wrapper.GetPixel(x, y);
			return (ColorF) new Color(c.A, c.R, c.G, c.B);
		}
	}
}