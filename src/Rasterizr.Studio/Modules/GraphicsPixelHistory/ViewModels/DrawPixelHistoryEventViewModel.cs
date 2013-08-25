using System.Collections.Generic;
using Rasterizr.Diagnostics;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
    public class DrawPixelHistoryEventViewModel : PixelHistoryEventViewModelBase
    {
        private readonly List<DrawPixelHistoryEventPartViewModel> _parts; 
        private readonly DrawEvent _event;

        public string Name
        {
            get { return string.Format("Triangle {0}", _event.PrimitiveID); }
        }

        public override ColorViewModel Result
        {
            get { return new ColorViewModel(_event.Result); }
        }

        public IEnumerable<DrawPixelHistoryEventPartViewModel> Parts
        {
            get { return _parts; }
        }

        public DrawPixelHistoryEventViewModel(DrawEvent @event)
        {
            _parts = new List<DrawPixelHistoryEventPartViewModel>
            {
                new VertexShaderDrawPixelHistoryEventPartViewModel(@event),
                new OutputMergerDrawPixelHistoryEventPartViewModel(@event)
            };
            _event = @event;
        }
    }
}