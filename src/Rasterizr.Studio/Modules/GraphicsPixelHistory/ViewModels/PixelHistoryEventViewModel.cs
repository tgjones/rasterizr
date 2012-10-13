using Caliburn.Micro;
using Nexus;
using Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
	public class PixelHistoryEventViewModel : PropertyChangedBase
	{
		private readonly ColorF _color;
		private readonly TracefileEventViewModel _event;

		public ColorF Color
		{
			get { return _color; }
		}

		public TracefileEventViewModel Event
		{
			get { return _event; }
		}

		public PixelHistoryEventViewModel(ColorF color, TracefileEventViewModel @event)
		{
			_color = color;
			_event = @event;
		}
	}
}