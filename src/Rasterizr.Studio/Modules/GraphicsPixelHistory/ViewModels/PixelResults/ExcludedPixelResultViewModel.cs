using Rasterizr.Diagnostics;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels.PixelResults
{
    public class ExcludedPixelResultViewModel : PixelResultViewModel
    {
        private readonly DrawEvent _event;

        public PixelExclusionReason ExclusionReason
        {
            get { return _event.ExclusionReason; }
        }

        public ExcludedPixelResultViewModel(DrawEvent @event)
        {
            _event = @event;
        }
    }
}