using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Caliburn.Micro;
using Rasterizr.Diagnostics.Logging;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Platform.Wpf;
using Rasterizr.Resources;

namespace Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels
{
	public class TracefileFrameViewModel : PropertyChangedBase, IDisposable
	{
		private readonly ISelectionService _selectionService;
		private readonly TracefileFrame _frame;
		private readonly IList<TracefileEventViewModel> _events;
	    private Texture2D _activeRenderTarget;

		internal TracefileFrame Model
		{
			get { return _frame; }
		}

		public int Number
		{
			get { return _frame.Number; }
		}

		public BitmapSource Image
		{
		    get
		    {
		        if (_activeRenderTarget == null)
		            return null;
		        return TextureLoader.CreateBitmapFromTexture(_activeRenderTarget, ActiveRenderTargetArraySlice, 0);
		    }
		}

        private int _activeRenderTargetViewIdentifier;
        public int ActiveRenderTargetViewIdentifier
        {
            get { return _activeRenderTargetViewIdentifier; }
            set
            {
                _activeRenderTargetViewIdentifier = value;
                NotifyOfPropertyChange(() => ActiveRenderTargetViewIdentifier);
            }
        }

        public IEnumerable<int> ActiveRenderTargetArraySlices
        {
            get
            {
                if (_activeRenderTarget == null)
                    return Enumerable.Empty<int>();
                return Enumerable.Range(0, _activeRenderTarget.Description.ArraySize);
            }
        }

        private int _activeRenderTargetArraySlice;
        public int ActiveRenderTargetArraySlice
        {
            get { return _activeRenderTargetArraySlice; }
            set
            {
                _activeRenderTargetArraySlice = value;
                NotifyOfPropertyChange(() => ActiveRenderTargetArraySlice);
                NotifyOfPropertyChange(() => Image);
            }
        }

		public IList<TracefileEventViewModel> Events
		{
			get { return _events; }
		}

		public TracefileFrameViewModel(ISelectionService selectionService, TracefileFrame frame)
		{
			_selectionService = selectionService;
			_frame = frame;
			_events = _frame.Events.Select(x => new TracefileEventViewModel(x)).ToList();

			_selectionService.SelectedEventChanged += OnSelectedEventChanged;
		}

		private void OnSelectedEventChanged(object sender, TracefileEventChangedEventArgs e)
		{
		    if (_selectionService.SelectedEvent == null)
		        return;

		    var dispatcher = Dispatcher.CurrentDispatcher;
            var swapChainPresenter = new WpfSwapChainPresenter(dispatcher);
		    var replayer = new Replayer(
		        _frame, _selectionService.SelectedEvent.Model,
		        swapChainPresenter);

		    Task.Factory.StartNew(() =>
		    {
                replayer.Replay();

		        DepthStencilView depthStencilView; RenderTargetView[] renderTargetViews;
                replayer.Device.ImmediateContext.OutputMerger.GetTargets(
                    out depthStencilView, out renderTargetViews);

		        var activeRenderTargetView = renderTargetViews[0];
		        _activeRenderTarget = (Texture2D) activeRenderTargetView.Resource;

		        ActiveRenderTargetViewIdentifier = activeRenderTargetView.ID;
                NotifyOfPropertyChange(() => ActiveRenderTargetArraySlices);
                ActiveRenderTargetArraySlice = 0;
		    });
		}

		public void Dispose()
		{
			_selectionService.SelectedEventChanged -= OnSelectedEventChanged;
		}
	}
}