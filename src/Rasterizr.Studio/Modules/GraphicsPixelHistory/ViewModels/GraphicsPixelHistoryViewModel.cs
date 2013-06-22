using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using System.ComponentModel.Composition;
using Rasterizr.Diagnostics.Logging;
using Rasterizr.Math;
using Rasterizr.Platform.Wpf;
using Rasterizr.Studio.Modules.GraphicsDebugging;

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

				FinalFrameBufferColor = new ColorViewModel(new Color4F(0, 1, 0, 1)); // TODO
			});
		}

		protected override void OnDeactivate(bool close)
		{
			_selectionService.SelectedPixelChanged -= OnSelectedPixelChanged;
			base.OnDeactivate(close);
		}
	}
}