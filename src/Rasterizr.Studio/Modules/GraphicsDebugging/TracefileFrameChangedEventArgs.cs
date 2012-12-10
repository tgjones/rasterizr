using System;
using Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels;

namespace Rasterizr.Studio.Modules.GraphicsDebugging
{
	public class TracefileFrameChangedEventArgs : EventArgs
	{
		private readonly TracefileFrameViewModel _tracefileFrameViewModel;

		public TracefileFrameViewModel TracefileFrameViewModel
		{
			get { return _tracefileFrameViewModel; }
		}

		public TracefileFrameChangedEventArgs(TracefileFrameViewModel tracefileFrameViewModel)
		{
			_tracefileFrameViewModel = tracefileFrameViewModel;
		}
	}
}