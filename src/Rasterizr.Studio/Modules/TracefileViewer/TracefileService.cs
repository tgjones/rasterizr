using System;
using System.ComponentModel.Composition;
using Rasterizr.Studio.Modules.TracefileViewer.ViewModels;

namespace Rasterizr.Studio.Modules.TracefileViewer
{
	[Export(typeof(ITracefileService))]
	public class TracefileService : ITracefileService
	{
		public event EventHandler<TracefileFrameChangedEventArgs> TracefileFrameChanged;

		public void NotifyTracefileFrameChanged(TracefileFrameViewModel tracefileViewModel)
		{
			OnTracefileChanged(new TracefileFrameChangedEventArgs(tracefileViewModel));
		}

		private void OnTracefileChanged(TracefileFrameChangedEventArgs e)
		{
			EventHandler<TracefileFrameChangedEventArgs> handler = TracefileFrameChanged;
			if (handler != null) handler(this, e);
		}
	}
}