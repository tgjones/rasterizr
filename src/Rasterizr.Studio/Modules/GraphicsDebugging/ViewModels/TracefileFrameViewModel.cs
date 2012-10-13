using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Rasterizr.Diagnostics.Logging;

namespace Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels
{
	public class TracefileFrameViewModel : PropertyChangedBase
	{
		private readonly TracefileFrame _frame;
		private readonly IList<TracefileEventViewModel> _events;

		public int Number
		{
			get { return _frame.Number; }
		}

		public IList<TracefileEventViewModel> Events
		{
			get { return _events; }
		}

		public TracefileFrameViewModel(TracefileFrame frame)
		{
			_frame = frame;
			_events = _frame.Events.Select(x => new TracefileEventViewModel(x)).ToList();
		}
	}
}