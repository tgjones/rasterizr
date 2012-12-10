using Rasterizr.Math;
using Rasterizr.Studio.Modules.GraphicsDebugging.Design;
using Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.Design
{
	public class DesignTimeGraphicsPixelHistoryViewModel : GraphicsPixelHistoryViewModel
	{
		public DesignTimeGraphicsPixelHistoryViewModel()
			: base(new DesignTimeSelectionService())
		{
			FinalFrameBufferColor = new ColorViewModel(new Color4F(0, 0, 1, 1));
		}
	}
}