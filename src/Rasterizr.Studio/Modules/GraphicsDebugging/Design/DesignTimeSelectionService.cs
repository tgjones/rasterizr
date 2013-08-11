using System;
using Rasterizr.Studio.Framework;
using Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels;

namespace Rasterizr.Studio.Modules.GraphicsDebugging.Design
{
	public class DesignTimeSelectionService : ISelectionService
	{
		public event EventHandler<TracefileFrameChangedEventArgs> SelectedFrameChanged;
		public event EventHandler<TracefileEventChangedEventArgs> SelectedEventChanged;
		public event EventHandler<PixelChangedEventArgs> SelectedPixelChanged;
		public TracefileFrameViewModel SelectedFrame { get; set; }
		public TracefileEventViewModel SelectedEvent { get; set; }
		public Int32Point SelectedPixel { get; set; }
	}
}