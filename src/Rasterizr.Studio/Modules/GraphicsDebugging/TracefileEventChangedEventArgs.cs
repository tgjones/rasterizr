using System;
using Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels;

namespace Rasterizr.Studio.Modules.GraphicsDebugging
{
	public class TracefileEventChangedEventArgs : EventArgs
	{
		private readonly TracefileEventViewModel _tracefileEventViewModel;

		public TracefileEventViewModel TracefileFrameViewModel
		{
			get { return _tracefileEventViewModel; }
		}

		public TracefileEventChangedEventArgs(TracefileEventViewModel tracefileEventViewModel)
		{
			_tracefileEventViewModel = tracefileEventViewModel;
		}
	}
}