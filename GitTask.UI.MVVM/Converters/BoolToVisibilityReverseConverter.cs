using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GitTask.UI.MVVM.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isVisible = value as bool?;
            if (isVisible == null) return false;

            return isVisible == true ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = value as Visibility?;

            return visibility == Visibility.Collapsed;
        }
    }
}