﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GitTask.UI.MVVM.Converters
{
    public class BoolToVisibilityReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isHidden = value as bool?;
            if (isHidden == null) return false;

            return isHidden == true ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = value as Visibility?;

            return visibility == Visibility.Hidden;
        }
    }
}
