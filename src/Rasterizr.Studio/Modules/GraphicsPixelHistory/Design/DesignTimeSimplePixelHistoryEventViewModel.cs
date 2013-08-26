using Rasterizr.Diagnostics;
using Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels;
using SlimShader;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.Design
{
    public class DesignTimeSimplePixelHistoryEventViewModel : SimplePixelHistoryEventViewModel
    {
        public DesignTimeSimplePixelHistoryEventViewModel()
            : base(new SimpleEvent(new Number4(0.3f, 0.234235443f, 1.0f, 0.0f)))
        {
        }
    }
}