using System.Windows;
using System.Windows.Input;
using GitTask.Domain.Model.Task;
using GitTask.UI.MVVM.View.TaskDetails;
using GitTask.UI.MVVM.ViewModel.TaskDetails;

namespace GitTask.UI.MVVM.View.Main
{
    public partial class TaskPartial
    {
        public TaskPartial()
        {
            InitializeComponent();
            Main.MouseDown += MainOnMouseDown;
            Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Main.MouseDown -= MainOnMouseDown;
        }

        private void MainOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var task = DataContext as Task;
            if (task == null) return;

            var taskDetailsWindow = new TaskDetailsWindow(new TaskDetailsViewModel(task));
            taskDetailsWindow.ShowDialog();
        }
    }
}