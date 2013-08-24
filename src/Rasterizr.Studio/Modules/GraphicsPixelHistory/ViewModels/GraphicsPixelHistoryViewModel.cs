using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using System.ComponentModel.Composition;
using Rasterizr.Diagnostics.Logging;
using Rasterizr.Platform.Wpf;
using Rasterizr.Studio.Framework;
using Rasterizr.Studio.Modules.GraphicsDebugging;
using SlimShader;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
	[Export(typeof(GraphicsPixelHistoryViewModel))]
	public class GraphicsPixelHistoryViewModel : Tool
	{
		private readonly ISelectionService _selectionService;

		public override PaneLocation PreferredLocation
		{
			get { return PaneLocation.Left; }
		}

        private bool _hasSelectedPixel;
        public bool HasSelectedPixel
        {
            get { return _hasSelectedPixel; }
            set
            {
                _hasSelectedPixel = value;
                NotifyOfPropertyChange(() => HasSelectedPixel);
            }
        }

		private ColorViewModel _finalFrameBufferColor;
		public ColorViewModel FinalFrameBufferColor
		{
			get { return _finalFrameBufferColor; }
			set
			{
				_finalFrameBufferColor = value;
				NotifyOfPropertyChange(() => FinalFrameBufferColor);
			}
		}

        private int _frameNumber;
        public int FrameNumber
        {
            get { return _frameNumber; }
            set
            {
                _frameNumber = value;
                NotifyOfPropertyChange(() => FrameNumber);
            }
        }

        private Int32Point _pixelLocation;
        public Int32Point PixelLocation
        {
            get { return _pixelLocation; }
            set
            {
                _pixelLocation = value;
                NotifyOfPropertyChange(() => PixelLocation);
            }
        }

		private readonly BindableCollection<PixelHistoryEventViewModel> _pixelEvents;
		public BindableCollection<PixelHistoryEventViewModel> PixelEvents
		{
			get { return _pixelEvents; }
		}

		[ImportingConstructor]
		public GraphicsPixelHistoryViewModel(ISelectionService selectionService)
		{
		    DisplayName = "Pixel History";

			_selectionService = selectionService;
			_pixelEvents = new BindableCollection<PixelHistoryEventViewModel>();
			selectionService.SelectedPixelChanged += OnSelectedPixelChanged;
		}

		private void OnSelectedPixelChanged(object sender, PixelChangedEventArgs e)
		{
		    FrameNumber = _selectionService.SelectedFrame.Number;
		    PixelLocation = e.SelectedPixel;

		    HasSelectedPixel = true;
			_pixelEvents.Clear();

			Task.Factory.StartNew(() =>
			{
				WpfSwapChain swapChain = null;
				var replayer = new Replayer(_selectionService.SelectedFrame.Model, _selectionService.SelectedEvent.Model, (d, desc) =>
				{
					Execute.OnUIThread(() => swapChain = new WpfSwapChain(d, desc.Width, desc.Height));
					return swapChain;
				}, true);
				replayer.Replay();

				var events = replayer.Logger.GetEvents(_selectionService.SelectedFrame.Number, e.SelectedPixel.X, e.SelectedPixel.Y);

				_pixelEvents.AddRange(events.Select(x => new PixelHistoryEventViewModel(x)));

			    FinalFrameBufferColor = _pixelEvents.Last().Color;
			});
		}

		protected override void OnDeactivate(bool close)
		{
			_selectionService.SelectedPixelChanged -= OnSelectedPixelChanged;
			base.OnDeactivate(close);
		}
	}
}