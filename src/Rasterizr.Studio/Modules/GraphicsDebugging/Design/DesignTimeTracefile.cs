using Rasterizr.Diagnostics;
using Rasterizr.Diagnostics.Logging.ObjectModel;

namespace Rasterizr.Studio.Modules.GraphicsDebugging.Design
{
	public class DesignTimeTracefile : Tracefile
	{
		public DesignTimeTracefile()
		{
			Frames.Add(
				new TracefileFrame
				{
					Number = 1,
					Events =
					{
						new TracefileEvent
						{
							Number = 1,
							OperationType = OperationType.InputAssemblerStageSetVertexBuffers
						}
					}
				});
		}
	}
}