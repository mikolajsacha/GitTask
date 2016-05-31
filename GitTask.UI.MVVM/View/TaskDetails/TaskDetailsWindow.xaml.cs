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
            AddCommentButton.Click += AddCommentButtonOnClick;
            AddCommentPopup.LostFocus += AddCommentPopupOnLostFocus;
        }

        private void AddCommentPopupOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            AddCommentPopup.IsOpen = false;
            if (!AddCommentPopupButton.IsMouseOver) return;

            var taskDetailsViewModel = DataContext as TaskDetailsViewModel;

            if (taskDetailsViewModel != null && taskDetailsViewModel.AddCommentCommand.CanExecute(new object()))
            {
                taskDetailsViewModel.AddCommentCommand.Execute(new object());
            }
        }

        private void AddCommentButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            AddCommentPopup.IsOpen = true;
            AddCommentPopup.Focus();
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