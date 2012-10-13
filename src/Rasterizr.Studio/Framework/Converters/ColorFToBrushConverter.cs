using System;
using System.Windows.Data;
using System.Windows.Media;
using Nexus;
using Color = Nexus.Color;

namespace Rasterizr.Studio.Framework.Converters
{
	public class ColorFToBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Color color = (Color) (ColorF)value;
			return new SolidColorBrush(System.Windows.Media.Color.FromArgb(
				color.A, color.R, color.G, color.B));
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
