using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace GitTask.UI.MVVM.Converters
{
    public class NameToInitialsConverter : IValueConverter
    {
        private const int MaxLength = 2;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var name = (string)value;
            if (name == null) return null;

            var splitName = name.Split(new[] { ' ' }, MaxLength, StringSplitOptions.RemoveEmptyEntries);
            if (splitName.Length == 1) // for one-word name, return two first letters
            {
                return name.Substring(0, Math.Min(name.Length, 2));
            }

            var sb = new StringBuilder();
            for (var i = 0; i < Math.Min(MaxLength, splitName.Length); i++)
            {
                sb.Append(splitName[i][0]);
            }
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
