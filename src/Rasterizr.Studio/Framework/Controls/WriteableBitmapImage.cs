using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Rasterizr.Studio.Framework.Controls
{
	public class WriteableBitmapImage : Image
	{
		#region Dependency properties

		public static readonly DependencyProperty WriteableBitmapProperty = DependencyProperty.Register("WriteableBitmap", typeof(WriteableBitmap), typeof(WriteableBitmapImage));

		public WriteableBitmap WriteableBitmap
		{
			get { return (WriteableBitmap) GetValue(WriteableBitmapProperty); }
			set { SetValue(WriteableBitmapProperty, value); }
		}

		#endregion

		private Size _size;

		protected override Size MeasureOverride(Size constraint)
		{
			// Fill available space.
			return constraint;
		}

		protected override Size ArrangeOverride(Size arrangeSize)
		{
			// If size has changed, recreate bitmap.
			if (arrangeSize != _size)
			{
				WriteableBitmap = new WriteableBitmap(
					(int) arrangeSize.Width, (int) arrangeSize.Height,
					96, 96, PixelFormats.Bgra32, null);
				Source = WriteableBitmap;
				_size = arrangeSize;
			}
			return arrangeSize;
		}
	}
}