using Rasterizr.Studio.Modules.GraphicsDebugging.Design;
using Rasterizr.Studio.Modules.TracefileViewer.ViewModels;

namespace Rasterizr.Studio.Modules.TracefileViewer.Design
{
	public class DesignTimeTracefileViewerViewModel : TracefileViewerViewModel
	{
		public DesignTimeTracefileViewerViewModel()
			: base(new DesignTimeSelectionService(), "test.trace", new DesignTimeTracefile())
		{
			HasHoverPixel = true;
			HasSelectedPixel = true;
		}
	}
}