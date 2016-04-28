using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace GitTask.UI.MVVM.Converters
{
    public class NameToInitialsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var name = (string)value;
            if (name == null) return null;

            var sb = new StringBuilder();
            foreach (var word in name.Split().Where(word => word.Length > 0))
            {
                sb.Append(char.ToUpper(word[0]));
            }
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
