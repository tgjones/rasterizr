using Rasterizr.Diagnostics;
using Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels.PixelResults;

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

        public PixelResultViewModel Result
        {
            get { return PixelResultViewModel.Create(_event); }
        }

        public ColorViewModel FinalColor
        {
            get
            {
                if (_event.Result == null)
                    return null;
                return new ColorViewModel(_event.Result.Value);
            }
        }

        public OutputMergerDrawPixelHistoryEventPartViewModel(DrawEvent @event)
        {
            _event = @event;
        }
    }
}