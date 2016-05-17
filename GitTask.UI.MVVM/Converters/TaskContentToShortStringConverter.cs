using System;
using System.Globalization;
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
                return taskContent.Substring(0, MaxLength - 3) + "...";
            }
            return taskContent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
