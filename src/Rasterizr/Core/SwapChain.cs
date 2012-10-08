using System.Windows.Media.Imaging;
using Nexus.Graphics;

namespace Rasterizr.Core
{
	public class SwapChain
	{
		private readonly RasterizrDevice _device;
		private readonly WriteableBitmapWrapper _writeableBitmap;
		private readonly ColorSurface _surface;

		public SwapChain(RasterizrDevice device, WriteableBitmap writeableBitmap, int width, int height, int multiSampleCount)
		{
			_device = device;
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
			_device.Loggers.EndFrame();
		}
	}
}