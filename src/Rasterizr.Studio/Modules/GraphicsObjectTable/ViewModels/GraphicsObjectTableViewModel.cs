using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Rasterizr.Diagnostics.Logging;
using Rasterizr.Platform.Wpf;
using Rasterizr.Studio.Modules.GraphicsDebugging;
using Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels;

namespace Rasterizr.Studio.Modules.GraphicsObjectTable.ViewModels
{
    [Export(typeof(GraphicsObjectTableViewModel))]
	public class GraphicsObjectTableViewModel : Tool
	{
		private readonly ISelectionService _selectionService;

		public override PaneLocation PreferredLocation
		{
			get { return PaneLocation.Bottom; }
		}

        public TracefileEventViewModel SelectedEvent
        {
            get { return _selectionService.SelectedEvent; }
        }

        private readonly BindableCollection<GraphicsObjectViewModel> _objects;
        public BindableCollection<GraphicsObjectViewModel> Objects
		{
            get { return _objects; }
		}

		[ImportingConstructor]
        public GraphicsObjectTableViewModel(ISelectionService selectionService)
		{
		    DisplayName = "Graphics Object Table";

			_selectionService = selectionService;
            _objects = new BindableCollection<GraphicsObjectViewModel>();
			selectionService.SelectedEventChanged += OnSelectedEventChanged;

            if (_selectionService.SelectedFrame != null && _selectionService.SelectedEvent != null)
                OnSelectedEventChanged(this, null);
		}

		private void OnSelectedEventChanged(object sender, TracefileEventChangedEventArgs e)
		{
            NotifyOfPropertyChange(() => SelectedEvent);

			_objects.Clear();

		    if (_selectionService.SelectedEvent == null)
		        return;

            var swapChainPresenter = new WpfSwapChainPresenter(Dispatcher.CurrentDispatcher);
            var replayer = new Replayer(
                _selectionService.SelectedFrame.Model, _selectionService.SelectedEvent.Model,
                swapChainPresenter);

			Task.Factory.StartNew(() =>
			{
				replayer.Replay();

				_objects.AddRange(replayer.Device.DeviceChildren
                    .Select(x => new GraphicsObjectViewModel(x)));
			});
		}

		protected override void OnDeactivate(bool close)
		{
            _selectionService.SelectedEventChanged -= OnSelectedEventChanged;
			base.OnDeactivate(close);
		}
	}
}