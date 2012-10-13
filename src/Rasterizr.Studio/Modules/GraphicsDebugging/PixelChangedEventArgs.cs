using System;
using Nexus;

namespace Rasterizr.Studio.Modules.GraphicsDebugging
{
	public class PixelChangedEventArgs : EventArgs
	{
		private readonly IntPoint2D _selectedPixel;

		public IntPoint2D SelectedPixel
		{
			get { return _selectedPixel; }
		}

		public PixelChangedEventArgs(IntPoint2D selectedPixel)
		{
			_selectedPixel = selectedPixel;
		}
	}
}