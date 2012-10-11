using Caliburn.Micro;
using Rasterizr.Diagnostics.Logging;

namespace Rasterizr.Studio.Modules.TracefileViewer.ViewModels
{
	public class TracefileFrameViewModel : PropertyChangedBase
	{
		private readonly TracefileFrame _frame;

		public TracefileFrame Frame
		{
			get { return _frame; }
		}

		public TracefileFrameViewModel(TracefileFrame frame)
		{
			_frame = frame;
		}
	}
}