using Rasterizr.Diagnostics;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
    public class OutputMergerDrawPixelHistoryEventPartViewModel : DrawPixelHistoryEventPartViewModel
    {
        private readonly DrawEvent _event;

        public override string Name
        {
            get { return "Output Merger"; }
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

        public OutputMergerDrawPixelHistoryEventPartViewModel(DrawEvent @event)
        {
            _event = @event;
        }
    }
}