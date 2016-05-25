using System.Windows;
using GitTask.UI.MVVM.ViewModel.TaskDetails;

namespace GitTask.UI.MVVM.View.TaskDetails
{
    public partial class TaskDetailsWindow
    {
        public TaskDetailsWindow(TaskDetailsViewModel dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
            EditButton.Click += EditButtonOnClick;
        }

        private void EditButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
            var taskDetailsViewModel = DataContext as TaskDetailsViewModel;

            if (taskDetailsViewModel != null && taskDetailsViewModel.EditTaskCommand.CanExecute(new object()))
            {
                taskDetailsViewModel.EditTaskCommand.Execute(new object());
            }
        }
    }
}