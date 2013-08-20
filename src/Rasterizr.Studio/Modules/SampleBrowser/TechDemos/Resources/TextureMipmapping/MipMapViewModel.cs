using System.Windows.Media.Imaging;
using Caliburn.Micro;

namespace Rasterizr.Studio.Modules.SampleBrowser.TechDemos.Resources.TextureMipmapping
{
	public class MipMapViewModel : PropertyChangedBase
	{
		private readonly WriteableBitmap _bitmap;
		public WriteableBitmap Bitmap
		{
			get { return _bitmap; }
		}

		private readonly string _description;
		public string Description
		{
			get { return _description; }
		}

		public MipMapViewModel(WriteableBitmap bitmap, int level)
		{
			_bitmap = bitmap;
			_description = string.Format("Mip level {0}: {1} x {2}", level, bitmap.PixelWidth, bitmap.PixelHeight);
		}
	}
}