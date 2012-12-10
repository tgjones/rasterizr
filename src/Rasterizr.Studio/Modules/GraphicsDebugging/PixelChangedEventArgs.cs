using System;
using Rasterizr.Math;

namespace Rasterizr.Studio.Modules.GraphicsDebugging
{
	public class PixelChangedEventArgs : EventArgs
	{
		private readonly Point _selectedPixel;

		public Point SelectedPixel
		{
			get { return _selectedPixel; }
		}

		public PixelChangedEventArgs(Point selectedPixel)
		{
			_selectedPixel = selectedPixel;
		}
	}
}