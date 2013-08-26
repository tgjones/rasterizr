using Caliburn.Micro;
using Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels.PixelResults;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
    public abstract class PixelHistoryEventViewModelBase : PropertyChangedBase
    {
		public abstract PixelResultViewModel Result { get; }
    }
}