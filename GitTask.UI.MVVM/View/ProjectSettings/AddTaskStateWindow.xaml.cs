using System.Windows;
using System.Windows.Media;
using GitTask.UI.MVVM.ViewModel.ProjectSettings;

namespace GitTask.UI.MVVM.View.ProjectSettings
{
    public partial class AddTaskStateWindow
    {
        public AddTaskStateWindow()
        {
            InitializeComponent();
            OkButton.Click += OkButtonOnClick;
        }

        private void OkButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
            var addTaskStateViewModel = DataContext as AddTaskStateViewModel;

            if (addTaskStateViewModel != null && addTaskStateViewModel.OkCommand.CanExecute(new object()))
            {
                addTaskStateViewModel.OkCommand.Execute(new object());
            }
        }

        private void ColorPicker_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            var addTaskStateViewModel = DataContext as AddTaskStateViewModel;

            if (addTaskStateViewModel != null)
            {
                addTaskStateViewModel.Brush = e.NewValue == null ? Brushes.Gray : new SolidColorBrush(e.NewValue.Value);
            }
        }
    }
}