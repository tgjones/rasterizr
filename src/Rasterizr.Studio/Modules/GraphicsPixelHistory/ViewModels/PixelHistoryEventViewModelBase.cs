using Caliburn.Micro;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
    public abstract class PixelHistoryEventViewModelBase : PropertyChangedBase
    {
		public abstract ColorViewModel Result { get; }
    }
}