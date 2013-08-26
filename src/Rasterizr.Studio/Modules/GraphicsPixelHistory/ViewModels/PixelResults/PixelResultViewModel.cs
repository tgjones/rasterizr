using Caliburn.Micro;
using Rasterizr.Diagnostics;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels.PixelResults
{
    public abstract class PixelResultViewModel : PropertyChangedBase
    {
        public static PixelResultViewModel Create(DrawEvent @event)
        {
            switch (@event.ExclusionReason)
            {
                case PixelExclusionReason.NotExcluded:
                    return new ColorResultViewModel(@event.Result.Value);
                default:
                    return new ExcludedPixelResultViewModel(@event);
            }
        }
    }
}