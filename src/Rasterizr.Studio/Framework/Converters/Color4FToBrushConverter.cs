using System;
using System.Windows.Data;
using System.Windows.Media;
using Rasterizr.Math;
using Color = Rasterizr.Math.Color4;

namespace Rasterizr.Studio.Framework.Converters
{
	public class Color4FToBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var color = ((Color4F) value).ToColor4();
			return new SolidColorBrush(System.Windows.Media.Color.FromArgb(
				color.A, color.R, color.G, color.B));
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
