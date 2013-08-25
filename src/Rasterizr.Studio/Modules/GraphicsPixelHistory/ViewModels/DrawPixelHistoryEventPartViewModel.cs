using Caliburn.Micro;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
    public abstract class DrawPixelHistoryEventPartViewModel : PropertyChangedBase
    {
        public abstract string Name { get; }
    }
}