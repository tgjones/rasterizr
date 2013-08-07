using Rasterizr.Studio.Modules.GraphicsDebugging.Design;
using Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels;
using SlimShader;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.Design
{
	public class DesignTimeGraphicsPixelHistoryViewModel : GraphicsPixelHistoryViewModel
	{
		public DesignTimeGraphicsPixelHistoryViewModel()
			: base(new DesignTimeSelectionService())
		{
            FinalFrameBufferColor = new ColorViewModel(new Number4(0, 0, 1, 1));
		}
	}
}