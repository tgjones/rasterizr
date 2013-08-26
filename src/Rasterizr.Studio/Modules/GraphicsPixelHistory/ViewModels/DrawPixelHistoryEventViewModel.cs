using System.Collections.Generic;
using Rasterizr.Diagnostics;
using Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels.PixelResults;

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

        public override PixelResultViewModel Result
        {
            get { return PixelResultViewModel.Create(_event); }
        }

        public PixelExclusionReason ExclusionReason
        {
            get { return _event.ExclusionReason; }
        }

        public IEnumerable<DrawPixelHistoryEventPartViewModel> Parts
        {
            get { return _parts; }
        }

        public DrawPixelHistoryEventViewModel(DrawEvent @event)
        {
            _parts = new List<DrawPixelHistoryEventPartViewModel>
            {
                new InputAssemblerDrawPixelHistoryEventPartViewModel(@event),
                new VertexShaderDrawPixelHistoryEventPartViewModel(@event),
                new OutputMergerDrawPixelHistoryEventPartViewModel(@event)
            };
            _event = @event;
        }
    }
}