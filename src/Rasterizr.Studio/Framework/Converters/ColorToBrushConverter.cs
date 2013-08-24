using System;
using System.Windows.Data;
using System.Windows.Media;
using Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels;
using Rasterizr.Toolkit;

namespace Rasterizr.Studio.Framework.Converters
{
	public class ColorToBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return null;

			var color = ((ColorViewModel) value).Color.ToColor();
			return new SolidColorBrush(System.Windows.Media.Color.FromArgb(
				color.A, color.R, color.G, color.B));
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
