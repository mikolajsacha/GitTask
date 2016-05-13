using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using GitTask.Repository.Model;

namespace GitTask.UI.MVVM.Converters
{
    public class ProjectMemberToBrushConverter : IValueConverter
    {
        private static readonly Brush DefaultBrush = Brushes.Gray;

        private static readonly Brush[] PossibleColors =
        {
            Brushes.DarkSlateGray,
            Brushes.LightSlateGray,
            Brushes.BlueViolet,
            Brushes.Purple,
            Brushes.DarkBlue,
            Brushes.DarkSlateBlue,
            Brushes.Sienna,
            Brushes.Brown,
            Brushes.Olive,
            Brushes.DarkOliveGreen,
            Brushes.Orchid,
            Brushes.LightSlateGray,
            Brushes.BurlyWood,
            Brushes.Teal,
            Brushes.SteelBlue
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var projectMember = value as ProjectMember;
            if (projectMember == null) return DefaultBrush;

            var str = projectMember.Name + projectMember.Email;
            try
            {
                var index = str.Sum(c => c) % PossibleColors.Length;
                return PossibleColors[index];
            }
            catch (OverflowException)
            {
                return DefaultBrush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
