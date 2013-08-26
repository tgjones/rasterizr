using System;
using System.Collections.Generic;
using System.Linq;
using Rasterizr.Diagnostics;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels;
using Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels.PixelResults;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
    public class PixelHistoryEventViewModel : TracefileEventViewModel
	{
		private readonly List<PixelHistoryEventViewModelBase> _pixelHistoryEvents;
        private readonly PixelResultViewModel _result;

        public PixelResultViewModel Result
        {
            get { return _result; }
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
					return new SimplePixelHistoryEventViewModel((SimpleEvent) x);
				if (x is DrawEvent)
					return new DrawPixelHistoryEventViewModel((DrawEvent) x);
				throw new ArgumentOutOfRangeException();
			}).ToList();
            
            _result = (_pixelHistoryEvents.LastOrDefault(x => x.Result is ColorResultViewModel) ?? _pixelHistoryEvents.Last()).Result;
		}
	}
}