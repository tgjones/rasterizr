using Rasterizr.Diagnostics;
using Rasterizr.Diagnostics.Logging.ObjectModel;
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
				OperationType = OperationType.InputAssemblerStageSetVertexBuffers
			}));
			Events.Add(new TracefileEventViewModel(new TracefileEvent
			{
				Number = 2,
				OperationType = OperationType.DeviceContextDraw
			}));
		}
	}
}