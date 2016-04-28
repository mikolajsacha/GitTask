using System;
using System.Globalization;
using System.Windows.Data;

namespace GitTask.UI.MVVM.Converters
{
    public class ToUpperCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var name = (string)value;
            return name?.ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
