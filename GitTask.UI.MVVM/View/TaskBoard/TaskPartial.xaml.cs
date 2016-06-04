using System.Windows;
using System.Windows.Input;
using GitTask.UI.MVVM.ViewModel.TaskDetails;

namespace GitTask.UI.MVVM.View.TaskBoard
{
    public partial class TaskPartial
    {
        public TaskPartial()
        {
            InitializeComponent();
            Content.MouseDown += MainOnMouseDown;
            Main.MouseLeave += MainOnMouseLeave;
            AddCommentButton.Click += AddCommentButtonOnClick;
            AddCommentPopup.LostFocus += AddCommentPopupOnLostFocus;
        }

        private void MainOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (!(mouseButtonEventArgs?.ClickCount > 1)) return;

            var taskDetails = DataContext as TaskDetailsViewModel;
            if (taskDetails == null) return;

            taskDetails.IsFullContentVisible = !taskDetails.IsFullContentVisible;
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

        private void AddCommentPopupOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            AddCommentPopup.IsOpen = false;

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
    }
}