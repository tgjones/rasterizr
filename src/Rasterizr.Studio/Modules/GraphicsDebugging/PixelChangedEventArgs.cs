using System;
using Rasterizr.Studio.Framework;

namespace Rasterizr.Studio.Modules.GraphicsDebugging
{
	public class PixelChangedEventArgs : EventArgs
	{
		private readonly Int32Point _selectedPixel;

		public Int32Point SelectedPixel
		{
			get { return _selectedPixel; }
		}

		public PixelChangedEventArgs(Int32Point selectedPixel)
		{
			_selectedPixel = selectedPixel;
		}
	}
}