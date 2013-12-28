using System.Collections.Generic;
using System.IO;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Studio.Framework;
using Rasterizr.Studio.Modules.GraphicsDebugging;
using Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels;
using Rasterizr.Studio.Modules.GraphicsEventList.ViewModels;
using Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels;

namespace Rasterizr.Studio.Modules.TracefileViewer.ViewModels
{
	public class TracefileViewerViewModel : Document
	{
		private readonly ISelectionService _selectionService;
		private readonly TracefileViewModel _tracefile;

		public IList<TracefileFrameViewModel> Frames
		{
			get { return _tracefile.Frames; }
		}

		public TracefileFrameViewModel SelectedFrame
		{
			get { return _selectionService.SelectedFrame; }
			set
			{
				_selectionService.SelectedFrame = value;
				NotifyOfPropertyChange(() => SelectedFrame);
			}
		}

	    public TracefileEventViewModel SelectedEvent
	    {
	        get { return _selectionService.SelectedEvent; }
	    }

		private Int32Point _hoverPixel;
		internal Int32Point HoverPixel
		{
			get { return _hoverPixel; }
			set
			{
				_hoverPixel = value;
				NotifyOfPropertyChange(() => HoverPixelDescription);
				HasHoverPixel = true;
			}
		}

		public string HoverPixelDescription
		{
			get
			{
				if (SelectedFrame.Image == null)
					return string.Empty;

				return string.Format("Hovered pixel X: {0} ({1:F3}) Y: {2} ({3:F3})",
					_hoverPixel.X, _hoverPixel.X / (float) SelectedFrame.Image.PixelWidth,
					_hoverPixel.Y, _hoverPixel.Y / (float) SelectedFrame.Image.PixelHeight);
			}
		}

		private bool _hasHoverPixel;
		public bool HasHoverPixel
		{
			get { return _hasHoverPixel; }
			set
			{
				_hasHoverPixel = value;
				NotifyOfPropertyChange(() => HasHoverPixel);
			}
		}

		private Int32Point _selectedPixel;
		internal Int32Point SelectedPixel
		{
			get { return _selectedPixel; }
			set
			{
				_selectedPixel = value;
				NotifyOfPropertyChange(() => SelectedPixelDescription);
				HasSelectedPixel = true;
				_selectionService.SelectedPixel = value;
			}
		}

		public string SelectedPixelDescription
		{
			get
			{
				if (SelectedFrame.Image == null)
					return string.Empty;

				return string.Format("Selected pixel X: {0} ({1:F3}) Y: {2} ({3:F3})",
					_selectedPixel.X, _selectedPixel.X / (float) SelectedFrame.Image.PixelWidth,
					_selectedPixel.Y, _selectedPixel.Y / (float) SelectedFrame.Image.PixelHeight);
			}
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

		public TracefileViewerViewModel(ISelectionService selectionService, string fileName, Tracefile tracefile)
		{
			_selectionService = selectionService;
		    _selectionService.SelectedEventChanged += OnSelectedEventChanged;

			_tracefile = new TracefileViewModel(selectionService, tracefile);
			_hasSelectedPixel = false;

		    DisplayName = Path.GetFileName(fileName);
		    SelectedFrame = _tracefile.Frames[0];
		}

	    private void OnSelectedEventChanged(object sender, TracefileEventChangedEventArgs e)
	    {
	        NotifyOfPropertyChange(() => SelectedEvent);
	    }

	    protected override void OnActivate()
	    {
            IoC.Get<IShell>().ShowTool(IoC.Get<GraphicsPixelHistoryViewModel>());
            IoC.Get<IShell>().ShowTool(IoC.Get<GraphicsEventListViewModel>());
	        base.OnActivate();
	    }

	    protected override void OnDeactivate(bool close)
	    {
	        _selectionService.SelectedEventChanged -= OnSelectedEventChanged;
	        base.OnDeactivate(close);
	    }
	}
}