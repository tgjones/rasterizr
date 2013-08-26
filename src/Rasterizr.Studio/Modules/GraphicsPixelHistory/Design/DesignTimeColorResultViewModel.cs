using Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels.PixelResults;
using SlimShader;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.Design
{
    public class DesignTimeColorResultViewModel : ColorResultViewModel
    {
        public DesignTimeColorResultViewModel() 
            : base(new Number4(0, 0.2f, 0.3f, 0.4f))
        {
        }
    }
}