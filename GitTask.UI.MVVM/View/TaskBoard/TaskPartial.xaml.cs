using System.Windows;
using System.Windows.Input;
using GitTask.UI.MVVM.View.TaskDetails;
using GitTask.UI.MVVM.ViewModel.TaskDetails;

namespace GitTask.UI.MVVM.View.TaskBoard
{
    public partial class TaskPartial
    {
        public TaskPartial()
        {
            InitializeComponent();
            Main.MouseDown += MainOnMouseDown;
            Main.MouseLeave += MainOnMouseLeave;
        }

        private void MainOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs == null || mouseButtonEventArgs.ClickCount <= 1) return;

            var taskDetails = DataContext as TaskDetailsViewModel;
            if (taskDetails == null) return;

            var taskDetailsWindow = new TaskDetailsWindow(taskDetails) {Owner = Application.Current.MainWindow};
            taskDetailsWindow.Show();
        }

        private void MainOnMouseLeave(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            var taskDetails = DataContext as TaskDetailsViewModel;
            if (taskDetails == null) return;

            var dependencyObject = sender as DependencyObject;
            if (dependencyObject == null) return;


            DragDrop.DoDragDrop(dependencyObject, taskDetails.Task.Title, DragDropEffects.Move);
        }
    }
}