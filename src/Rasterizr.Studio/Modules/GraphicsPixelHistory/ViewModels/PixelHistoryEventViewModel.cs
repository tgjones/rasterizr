using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Rasterizr.Diagnostics;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Math;
using Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
	public abstract class PixelHistoryEventViewModelBase : PropertyChangedBase
	{
		
	}

	public class SimplePixelHistoryEventViewModel : PixelHistoryEventViewModelBase
	{
		private readonly ColorViewModel _result;

		public ColorViewModel Result
		{
			get { return _result; }
		}

		public SimplePixelHistoryEventViewModel(Color4F result)
		{
			_result = new ColorViewModel(result);
		}
	}

	public class DrawPixelHistoryEventViewModel : PixelHistoryEventViewModelBase
	{
		private readonly DrawEvent _event;

		public string Name
		{
			get { return string.Format("Triangle {0}", _event.PrimitiveID); }
		}

		public PixelExclusionReason ExclusionReason
		{
			get { return _event.ExclusionReason; }
		}

		public ColorViewModel Previous
		{
			get { return new ColorViewModel(_event.Previous); }
		}

		public ColorViewModel PixelShader
		{
			get { return new ColorViewModel(_event.PixelShader); }
		}

		public ColorViewModel Result
		{
			get { return new ColorViewModel(_event.Result); }
		}

		public DrawPixelHistoryEventViewModel(DrawEvent @event)
		{
			_event = @event;
		}
	}

	public class PixelHistoryEventViewModel : TracefileEventViewModel
	{
		private readonly IEnumerable<PixelHistoryEventViewModelBase> _pixelHistoryEvents;

		public IEnumerable<PixelHistoryEventViewModelBase> PixelHistoryEvents
		{
			get { return _pixelHistoryEvents; }
		}

		public PixelHistoryEventViewModel(TracefileEvent tracefileEvent)
			: base(tracefileEvent)
		{
			_pixelHistoryEvents = tracefileEvent.PixelEvents.Select<PixelEvent, PixelHistoryEventViewModelBase>(x =>
			{
				if (x is ClearRenderTargetEvent)
					return new SimplePixelHistoryEventViewModel(((ClearRenderTargetEvent) x).Result);
				if (x is DrawEvent)
					return new DrawPixelHistoryEventViewModel((DrawEvent) x);
				throw new ArgumentOutOfRangeException();
			});
		}
	}
}