using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using System.ComponentModel.Composition;
using Rasterizr.Diagnostics.Logging;
using Rasterizr.Platform.Wpf;
using Rasterizr.Studio.Framework;
using Rasterizr.Studio.Modules.GraphicsDebugging;
using Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels.PixelResults;

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

            var swapChainPresenter = new WpfSwapChainPresenter(Dispatcher.CurrentDispatcher);
            var replayer = new Replayer(
                _selectionService.SelectedFrame.Model, _selectionService.SelectedEvent.Model,
                swapChainPresenter, 
                _selectionService.SelectedFrame.ActiveRenderTargetViewIdentifier,
                _selectionService.SelectedFrame.ActiveRenderTargetArraySlice,
                e.SelectedPixel.X, e.SelectedPixel.Y);

			Task.Factory.StartNew(() =>
			{
				replayer.Replay();

				var events = replayer.Logger.GetPixelHistoryEvents(_selectionService.SelectedFrame.Number);
				_pixelEvents.AddRange(events.Select(x => new PixelHistoryEventViewModel(x)));

			    FinalFrameBufferColor = ((ColorResultViewModel) _pixelEvents.Last(x => x.Result is ColorResultViewModel).Result).Result;
			});
		}

		protected override void OnDeactivate(bool close)
		{
			_selectionService.SelectedPixelChanged -= OnSelectedPixelChanged;
			base.OnDeactivate(close);
		}
	}
}