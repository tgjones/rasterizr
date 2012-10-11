using Caliburn.Micro;
using Rasterizr.Diagnostics.Logging;

namespace Rasterizr.Studio.Modules.GraphicsEventList.ViewModels
{
	public class TracefileEventViewModel : PropertyChangedBase
	{
		private readonly TracefileEvent _tracefileEvent;

		public int Number
		{
			get { return _tracefileEvent.Number; }
		}

		public TracefileEventViewModel(TracefileEvent tracefileEvent)
		{
			_tracefileEvent = tracefileEvent;
		}
	}
}