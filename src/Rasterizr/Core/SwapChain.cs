using System.Windows.Media.Imaging;
using Nexus.Graphics;

namespace Rasterizr.Core
{
	public class SwapChain
	{
		private readonly WriteableBitmapWrapper _writeableBitmap;
		private readonly ColorSurface _surface;

		public SwapChain(WriteableBitmap writeableBitmap, int width, int height, int multiSampleCount)
		{
			_writeableBitmap = new WriteableBitmapWrapper(writeableBitmap);
			_surface = new ColorSurface(width, height, multiSampleCount);
		}

		public ColorSurface GetBuffer()
		{
			return _surface;
		}

		public void Present()
		{
			_surface.Resolve(new WriteableBitmapBuffer(_writeableBitmap));
			_writeableBitmap.Invalidate();
		}
	}
}