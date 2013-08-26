using Rasterizr.Diagnostics;
using Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels.PixelResults;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
    public class SimplePixelHistoryEventViewModel : PixelHistoryEventViewModelBase
    {
        private readonly ColorResultViewModel _result;

        public override PixelResultViewModel Result
        {
            get { return _result; }
        }

        public ColorViewModel Color
        {
            get { return _result.Result;}
        }

        public SimplePixelHistoryEventViewModel(SimpleEvent @event)
        {
            _result = new ColorResultViewModel(@event.Result);
        }
    }
}