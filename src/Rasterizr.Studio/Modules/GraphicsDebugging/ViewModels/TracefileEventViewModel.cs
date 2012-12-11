using Caliburn.Micro;
using Rasterizr.Diagnostics;
using Rasterizr.Diagnostics.Logging.ObjectModel;

namespace Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels
{
	public class TracefileEventViewModel : PropertyChangedBase
	{
		private readonly TracefileEvent _tracefileEvent;

		internal TracefileEvent Model
		{
			get { return _tracefileEvent; }
		}

		public int Number
		{
			get { return _tracefileEvent.Number; }
		}

		public OperationType OperationType
		{
			get { return _tracefileEvent.OperationType; }
		}

		public TracefileEventViewModel(TracefileEvent tracefileEvent)
		{
			_tracefileEvent = tracefileEvent;
		}
	}
}