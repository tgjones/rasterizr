using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Rasterizr.Diagnostics.Logging.ObjectModel;

namespace Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels
{
	public class TracefileViewModel : PropertyChangedBase
	{
		private readonly Tracefile _tracefile;
		private readonly IList<TracefileFrameViewModel> _frames;

		public IList<TracefileFrameViewModel> Frames
		{
			get { return _frames; }
		}

		public TracefileViewModel(ISelectionService selectionService, Tracefile tracefile)
		{
			_tracefile = tracefile;
			_frames = _tracefile.Frames.Select(x => new TracefileFrameViewModel(selectionService, x)).ToList();
		}
	}
}