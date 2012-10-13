using Rasterizr.Diagnostics.Logging;
using Rasterizr.Studio.Modules.GraphicsDebugging.Design;
using Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels;
using Rasterizr.Studio.Modules.GraphicsEventList.ViewModels;

namespace Rasterizr.Studio.Modules.GraphicsEventList.Design
{
	public class DesignTimeGraphicsEventListViewModel : GraphicsEventListViewModel
	{
		public DesignTimeGraphicsEventListViewModel()
			: base(new DesignTimeSelectionService())
		{
			Events.Add(new TracefileEventViewModel(new TracefileEvent
			{
				Number = 1,
				OperationType = Core.Diagnostics.OperationType.InputAssemblerSetVertices
			}));
			Events.Add(new TracefileEventViewModel(new TracefileEvent
			{
				Number = 2,
				OperationType = Core.Diagnostics.OperationType.DeviceDraw
			}));
		}
	}
}