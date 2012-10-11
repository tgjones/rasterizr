using System;
using Rasterizr.Studio.Modules.TracefileViewer.ViewModels;

namespace Rasterizr.Studio.Modules.TracefileViewer
{
	public interface ITracefileService
	{
		event EventHandler<TracefileFrameChangedEventArgs> TracefileFrameChanged;
		void NotifyTracefileFrameChanged(TracefileFrameViewModel tracefileFrameViewModel);
	}
}