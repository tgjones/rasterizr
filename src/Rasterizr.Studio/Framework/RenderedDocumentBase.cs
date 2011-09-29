using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Nexus.Graphics;
using Rasterizr.Core;
using Rasterizr.Core.OutputMerger;

namespace Rasterizr.Studio.Framework
{
	public abstract class RenderedDocumentBase : Screen
	{
		private readonly RasterizrDevice _device;
		private SwapChain _swapChain;

		protected RasterizrDevice Device
		{
			get { return _device; }
		}

		protected SwapChain SwapChain
		{
			get { return _swapChain; }
		}

		private WriteableBitmap _outputBitmap;
		public WriteableBitmap OutputBitmap
		{
			get { return _outputBitmap; }
			set
			{
				_outputBitmap = value;
				if (value != null)
				{
					RecreateSwapChain();
					Draw();
				}
			}
		}

		protected RenderedDocumentBase()
 		{
			_device = new RasterizrDevice();
 		}

		private void RecreateSwapChain()
		{
			_swapChain = new SwapChain(OutputBitmap, (int)OutputBitmap.Width, (int)OutputBitmap.Height, 1);
			_device.Rasterizer.Viewport = new Viewport3D(0, 0, (int)OutputBitmap.Width, (int)OutputBitmap.Height);
			_device.OutputMerger.RenderTarget = new RenderTargetView(_swapChain.GetBuffer());
		}

		protected abstract void Draw();
	}
}