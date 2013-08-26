using System.Collections.Generic;
using Rasterizr.Diagnostics;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Studio.Framework;
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
		    HasSelectedPixel = true;
            FinalFrameBufferColor = new ColorViewModel(new Number4(0, 0, 1, 1));

		    FrameNumber = 2;
            PixelLocation = new Int32Point(200, 305);

		    PixelEvents.AddRange(new[]
		    {
                new PixelHistoryEventViewModel(new TracefileEvent
                {
                    Number = 1,
                    OperationType = OperationType.DeviceContextClearRenderTargetView,
                    PixelEvents = new List<PixelEvent>(new[] { new SimpleEvent(new Number4(1, 0, 0, 1)) })
                }), 
                new PixelHistoryEventViewModel(new TracefileEvent
                {
                    Number = 15,
                    OperationType = OperationType.DeviceContextDraw,
                    PixelEvents = new List<PixelEvent>(new[]
                    {
                        DesignTimeDrawPixelHistoryEventViewModel.CreateDrawEvent(),
                        DesignTimeDrawPixelHistoryEventViewModel.CreateDrawEvent(PixelExclusionReason.FailedDepthTest)
                    })
                }), 
		    });
		}
	}
}