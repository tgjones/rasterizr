using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using System.ComponentModel.Composition;
using Nexus.Graphics.Colors;
using Rasterizr.Studio.Modules.GraphicsDebugging;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
	[Export(typeof(GraphicsPixelHistoryViewModel))]
	public class GraphicsPixelHistoryViewModel : Tool
	{
		public override string DisplayName
		{
			get { return "Pixel History"; }
		}

		public override PaneLocation PreferredLocation
		{
			get { return PaneLocation.Left; }
		}

		private ColorF _finalFrameBufferColor;
		public ColorF FinalFrameBufferColor
		{
			get { return _finalFrameBufferColor; }
			set
			{
				_finalFrameBufferColor = value;
				NotifyOfPropertyChange(() => FinalFrameBufferColor);
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
			_pixelEvents = new BindableCollection<PixelHistoryEventViewModel>();
			selectionService.SelectedPixelChanged += OnSelectedPixelChanged;
		}

		private void OnSelectedPixelChanged(object sender, PixelChangedEventArgs e)
		{
			_pixelEvents.Clear();
			
			// TODO: Fire async request to get pixel history.
		}
	}
}