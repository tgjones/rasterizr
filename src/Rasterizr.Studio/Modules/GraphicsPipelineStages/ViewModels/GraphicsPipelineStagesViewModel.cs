using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Threading;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Rasterizr.Diagnostics.Logging;
using Rasterizr.Platform.Wpf;
using Rasterizr.Studio.Modules.GraphicsDebugging;
using Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels;

namespace Rasterizr.Studio.Modules.GraphicsPipelineStages.ViewModels
{
    [Export(typeof(GraphicsPipelineStagesViewModel))]
	public class GraphicsPipelineStagesViewModel : Tool
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

        private readonly BindableCollection<VertexViewModel> _inputAssemblerOutputs;
        public IObservableCollection<VertexViewModel> InputAssemblerOutputs
        {
            get { return _inputAssemblerOutputs; }
        }
        
        [ImportingConstructor]
        public GraphicsPipelineStagesViewModel(ISelectionService selectionService)
		{
		    DisplayName = "Graphics Pipeline Stages";

            _inputAssemblerOutputs = new BindableCollection<VertexViewModel>();

			_selectionService = selectionService;
			selectionService.SelectedEventChanged += OnSelectedEventChanged;

            if (_selectionService.SelectedFrame != null && _selectionService.SelectedEvent != null)
                OnSelectedEventChanged(this, null);
		}

		private void OnSelectedEventChanged(object sender, TracefileEventChangedEventArgs e)
		{
            NotifyOfPropertyChange(() => SelectedEvent);

		    if (_selectionService.SelectedEvent == null)
		        return;

            var swapChainPresenter = new WpfSwapChainPresenter(Dispatcher.CurrentDispatcher);
            var replayer = new Replayer(
                _selectionService.SelectedFrame.Model, _selectionService.SelectedEvent.Model,
                swapChainPresenter);

            //Task.Factory.StartNew(() =>
            //{
            //    replayer.Replay();

				
            //});
		}

		protected override void OnDeactivate(bool close)
		{
            _selectionService.SelectedEventChanged -= OnSelectedEventChanged;
			base.OnDeactivate(close);
		}
	}

    public class VertexViewModel
    {
        public int Number
        {
            get { return 0; }
        }


    }
}