using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Rasterizr.Studio.Modules.TracefileViewer;

namespace Rasterizr.Studio.Modules.GraphicsEventList.ViewModels
{
	[Export(typeof(GraphicsEventListViewModel))]
	public class GraphicsEventListViewModel : Tool
	{
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

		[ImportingConstructor]
		public GraphicsEventListViewModel(ITracefileService tracefileService)
		{
			_events = new BindableCollection<TracefileEventViewModel>();
			tracefileService.TracefileFrameChanged += OnTracefileFrameChanged;
		}

		private void OnTracefileFrameChanged(object sender, TracefileFrameChangedEventArgs e)
		{
			_events.Clear();
			_events.AddRange(e.TracefileFrameViewModel.Frame.Events
				.Select(x => new TracefileEventViewModel(x)));
		}
	}
}