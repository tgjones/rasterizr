using Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.Design
{
    public class DesignTimeOutputMergerDrawPixelHistoryEventPartViewModel : OutputMergerDrawPixelHistoryEventPartViewModel
    {
        public DesignTimeOutputMergerDrawPixelHistoryEventPartViewModel()
            : base(DesignTimeDrawPixelHistoryEventViewModel.CreateDrawEvent())
        {
            
        }
    }
}