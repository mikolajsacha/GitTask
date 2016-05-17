using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using GitTask.Domain.Enum;

namespace GitTask.UI.MVVM.Converters
{
    public class TaskPriorityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TaskPriority?)) return null;
            var priority = (TaskPriority?)value;

            switch (priority)
            {
                case TaskPriority.Minor:
                    return Brushes.Blue;
                case TaskPriority.Medium:
                    return Brushes.Green;
                case TaskPriority.Major:
                    return Brushes.Orange;
                case TaskPriority.Blocker:
                    return Brushes.Red;
                case TaskPriority.Critical:
                    return Brushes.DarkRed;
                default:
                    return Brushes.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
