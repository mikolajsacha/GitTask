using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GitTask.UI.MVVM.Converters
{
    public class HexToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hex = value as string;
            if (hex == null) return null;
            var converter = new BrushConverter();
            return (Brush)converter.ConvertFromString(hex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((SolidColorBrush)value).Color.ToString();
        }
    }
}
