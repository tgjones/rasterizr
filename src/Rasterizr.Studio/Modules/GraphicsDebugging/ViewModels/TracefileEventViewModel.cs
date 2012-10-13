using Caliburn.Micro;
using Rasterizr.Core.Diagnostics;
using Rasterizr.Diagnostics.Logging;

namespace Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels
{
	public class TracefileEventViewModel : PropertyChangedBase
	{
		private readonly TracefileEvent _tracefileEvent;

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