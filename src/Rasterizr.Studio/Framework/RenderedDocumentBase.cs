using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Modules.Output;
using Nexus.Graphics;
using Rasterizr.Diagnostics;
using Rasterizr.Diagnostics.Logging;
using Rasterizr.OutputMerger;

namespace Rasterizr.Studio.Framework
{
	public abstract class RenderedDocumentBase : Document
	{
		private readonly RasterizrDevice _device;
		private SwapChain _swapChain;

		protected TracefileGraphicsLogger _logger;

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
			_logger = new TracefileGraphicsLogger(new StreamWriter("test.trace"));
			_device = new RasterizrDevice(new List<GraphicsLogger>
			{
				new TextWriterGraphicsLogger(IoC.Get<IOutput>().Writer),
				_logger
			});
 		}

		private void RecreateSwapChain()
		{
			_swapChain = new SwapChain(_device, OutputBitmap, (int)OutputBitmap.Width, (int)OutputBitmap.Height, 1);
			_device.Rasterizer.Viewport = new Viewport3D(0, 0, (int)OutputBitmap.Width, (int)OutputBitmap.Height);
			_device.OutputMerger.RenderTarget = new RenderTargetView(_swapChain.GetBuffer());
		}

		protected abstract void Draw();
	}
}