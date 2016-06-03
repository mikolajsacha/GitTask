using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GitTask.UI.MVVM.View.TaskDetails;
using GitTask.UI.MVVM.ViewModel.TaskDetails;

namespace GitTask.UI.MVVM.View.TaskBoard
{
    public partial class TaskPartial
    {
        private readonly RelayCommand _showDetailsCommand;
        public ICommand ShowDetailsCommand => _showDetailsCommand;

        public TaskPartial()
        {
            _showDetailsCommand = new RelayCommand(ShowTaskDetails);
            InitializeComponent();
            Main.MouseDown += MainOnMouseDown;
            Main.MouseLeave += MainOnMouseLeave;
        }

        private void MainOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs?.ClickCount > 1)
            {
                ShowTaskDetails();
            }
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

        private void ShowTaskDetails()
        {
            var taskDetails = DataContext as TaskDetailsViewModel;
            if (taskDetails == null) return;

            var taskDetailsWindow = new TaskDetailsWindow(taskDetails) { Owner = Application.Current.MainWindow };
            taskDetailsWindow.Show();
        }
    }
}