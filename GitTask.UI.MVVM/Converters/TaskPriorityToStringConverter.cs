using System;
using System.Globalization;
using System.Windows.Data;
using GitTask.Domain.Enum;
using GitTask.UI.MVVM.Properties;

namespace GitTask.UI.MVVM.Converters
{
    public class TaskPriorityToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TaskPriority)) return null;
            var priority = (TaskPriority)value;

            switch (priority)
            {
                case TaskPriority.Minor:
                    return Resources.PriorityMinor;
                case TaskPriority.Medium:
                    return Resources.PriorityMedium;
                case TaskPriority.Major:
                    return Resources.PriorityMajor;
                case TaskPriority.Blocker:
                    return Resources.PriorityBlocker;
                case TaskPriority.Critical:
                    return Resources.PriorityCritical;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
