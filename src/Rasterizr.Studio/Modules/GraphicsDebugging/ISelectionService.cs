using System;
using Rasterizr.Studio.Framework;
using Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels;

namespace Rasterizr.Studio.Modules.GraphicsDebugging
{
	public interface ISelectionService
	{
		event EventHandler<TracefileFrameChangedEventArgs> SelectedFrameChanged;
		event EventHandler<TracefileEventChangedEventArgs> SelectedEventChanged;
		event EventHandler<PixelChangedEventArgs> SelectedPixelChanged;

		TracefileFrameViewModel SelectedFrame { get; set; }
		TracefileEventViewModel SelectedEvent { get; set; }
		Int32Point SelectedPixel { get; set; }
	}
}