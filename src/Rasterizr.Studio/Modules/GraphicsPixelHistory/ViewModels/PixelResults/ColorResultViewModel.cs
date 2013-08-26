using SlimShader;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels.PixelResults
{
    public class ColorResultViewModel : PixelResultViewModel
    {
        private readonly ColorViewModel _result;

        public ColorViewModel Result
        {
            get { return _result; }
        }

        public ColorResultViewModel(Number4 color)
        {
            _result = new ColorViewModel(color);
        }
    }
}