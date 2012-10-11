using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Modules.Output;
using Nexus.Graphics;
using Rasterizr.Core;
using Rasterizr.Core.Diagnostics;
using Rasterizr.Core.OutputMerger;
using Rasterizr.Diagnostics.Logging;

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
			_device = new RasterizrDevice();
			_device.Loggers.Add(new TextWriterGraphicsLogger(IoC.Get<IOutput>().Writer));

			_logger = new TracefileGraphicsLogger(new StreamWriter("test.trace"));
			_device.Loggers.Add(_logger);
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