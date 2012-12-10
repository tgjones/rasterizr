using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Rasterizr.Diagnostics.Logging;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Platform.Wpf;

namespace Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels
{
	public class TracefileFrameViewModel : PropertyChangedBase
	{
		private readonly TracefileFrame _frame;
		private readonly IList<TracefileEventViewModel> _events;

		public int Number
		{
			get { return _frame.Number; }
		}

		private BitmapSource _image;
		public BitmapSource Image
		{
			get { return _image; }
			set
			{
				_image = value;
				NotifyOfPropertyChange(() => Image);
			}
		}

		public IList<TracefileEventViewModel> Events
		{
			get { return _events; }
		}

		public TracefileFrameViewModel(TracefileFrame frame)
		{
			_frame = frame;
			_events = _frame.Events.Select(x => new TracefileEventViewModel(x)).ToList();

			Task.Factory.StartNew(() =>
			{
				WpfSwapChain swapChain = null;
				var replayer = new Replayer(frame, (d, desc) =>
				{
					Execute.OnUIThread(() => swapChain = new WpfSwapChain(d, desc.Width, desc.Height));
					return swapChain;
				});
				replayer.Replay();
				Image = swapChain.Bitmap;
			});
		}
	}
}