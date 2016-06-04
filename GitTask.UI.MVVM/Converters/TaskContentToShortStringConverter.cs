using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace GitTask.UI.MVVM.Converters
{
    public class TaskContentToShortStringConverter : IValueConverter
    {
        private const int MaxLength = 50;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var taskContent = (string)value;
            if (taskContent == null) return null;

            if (taskContent.Length > MaxLength)
            {
                taskContent = taskContent.Substring(0, MaxLength - 3) + "...";
            }
            var splitContent = taskContent.Split('\n').Take(2);
            var mergedContent = new StringBuilder();
            foreach (var word in splitContent) mergedContent.Append(word);
            return mergedContent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
