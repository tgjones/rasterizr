using System.ComponentModel.Composition;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Rasterizr.Studio.Modules.GraphicsDebugging;
using Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels;

namespace Rasterizr.Studio.Modules.GraphicsEventList.ViewModels
{
	[Export(typeof(GraphicsEventListViewModel))]
	public class GraphicsEventListViewModel : Tool
	{
		private readonly ISelectionService _selectionService;

		public override PaneLocation PreferredLocation
		{
			get { return PaneLocation.Right; }
		}

		private readonly BindableCollection<TracefileEventViewModel> _events;
		public BindableCollection<TracefileEventViewModel> Events
		{
			get { return _events; }
		}

		public TracefileEventViewModel SelectedEvent
		{
			get { return _selectionService.SelectedEvent; }
			set
			{
				_selectionService.SelectedEvent = value;
				NotifyOfPropertyChange(() => SelectedEvent);
			}
		}

		[ImportingConstructor]
		public GraphicsEventListViewModel(ISelectionService selectionService)
		{
		    DisplayName = "Graphics Event List";

			_selectionService = selectionService;
			_events = new BindableCollection<TracefileEventViewModel>();
			selectionService.SelectedFrameChanged += OnSelectedFrameChanged;
			OnSelectedFrameChanged(this, new TracefileFrameChangedEventArgs(selectionService.SelectedFrame));
		}

		private void OnSelectedFrameChanged(object sender, TracefileFrameChangedEventArgs e)
		{
			_events.Clear();
			if (e.TracefileFrameViewModel != null)
				_events.AddRange(e.TracefileFrameViewModel.Events);
		}
	}
}