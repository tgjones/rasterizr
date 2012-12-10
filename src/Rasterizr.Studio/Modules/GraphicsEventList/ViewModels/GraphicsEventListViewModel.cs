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

		public override string DisplayName
		{
			get { return "Graphics Event List"; }
		}

		public override PaneLocation PreferredLocation
		{
			get { return PaneLocation.Right; }
		}

		private readonly BindableCollection<TracefileEventViewModel> _events;
		public BindableCollection<TracefileEventViewModel> Events
		{
			get { return _events; }
		}

		private TracefileEventViewModel _selectedEvent;
		public TracefileEventViewModel SelectedEvent
		{
			get { return _selectedEvent; }
			set
			{
				_selectedEvent = value;
				_selectionService.SelectedEvent = value;
				NotifyOfPropertyChange(() => SelectedEvent);
			}
		}

		[ImportingConstructor]
		public GraphicsEventListViewModel(ISelectionService selectionService)
		{
			_selectionService = selectionService;
			_events = new BindableCollection<TracefileEventViewModel>();
			selectionService.SelectedFrameChanged += OnSelectedFrameChanged;
		}

		private void OnSelectedFrameChanged(object sender, TracefileFrameChangedEventArgs e)
		{
			_events.Clear();
			_events.AddRange(e.TracefileFrameViewModel.Events);
		}
	}
}