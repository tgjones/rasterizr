using SlimShader;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
    public class SimplePixelHistoryEventViewModel : PixelHistoryEventViewModelBase
    {
        private readonly ColorViewModel _result;

        public override ColorViewModel Result
        {
            get { return _result; }
        }

        public SimplePixelHistoryEventViewModel(Number4 result)
        {
            _result = new ColorViewModel(result);
        }
    }
}