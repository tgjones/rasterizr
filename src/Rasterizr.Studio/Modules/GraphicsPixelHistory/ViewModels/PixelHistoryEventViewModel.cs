using Caliburn.Micro;
using Rasterizr.Math;
using Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
	public class PixelHistoryEventViewModel : PropertyChangedBase
	{
		private readonly ColorViewModel _color;
		private readonly TracefileEventViewModel _event;

		public ColorViewModel Color
		{
			get { return _color; }
		}

		public TracefileEventViewModel Event
		{
			get { return _event; }
		}

		public PixelHistoryEventViewModel(ColorViewModel color, TracefileEventViewModel @event)
		{
			_color = color;
			_event = @event;
		}
	}
}