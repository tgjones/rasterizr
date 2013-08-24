using System;
using System.Collections.Generic;
using System.Linq;
using Rasterizr.Diagnostics;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
    public class PixelHistoryEventViewModel : TracefileEventViewModel
	{
		private readonly IEnumerable<PixelHistoryEventViewModelBase> _pixelHistoryEvents;
        private readonly ColorViewModel _color;

        public ColorViewModel Color
        {
            get { return _color; }
        }

		public IEnumerable<PixelHistoryEventViewModelBase> PixelHistoryEvents
		{
			get { return _pixelHistoryEvents; }
		}

		public PixelHistoryEventViewModel(TracefileEvent tracefileEvent)
			: base(tracefileEvent)
		{
			_pixelHistoryEvents = tracefileEvent.PixelEvents.Select<PixelEvent, PixelHistoryEventViewModelBase>(x =>
			{
				if (x is SimpleEvent)
					return new SimplePixelHistoryEventViewModel(((SimpleEvent) x).Result);
				if (x is DrawEvent)
					return new DrawPixelHistoryEventViewModel((DrawEvent) x);
				throw new ArgumentOutOfRangeException();
			});
		    _color = _pixelHistoryEvents.Last().Result;
		}
	}
}