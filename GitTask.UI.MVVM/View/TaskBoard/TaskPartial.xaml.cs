using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using GitTask.UI.MVVM.Locator;
using GitTask.UI.MVVM.Messages;
using GitTask.UI.MVVM.ViewModel.TaskDetails;

namespace GitTask.UI.MVVM.View.TaskBoard
{
    public partial class TaskPartial
    {
        private const double DefaultWidth = 250;

        public TaskPartial()
        {
            InitializeComponent();
            ContentGrid.MouseDown += MainOnMouseDown;
            CommentsPanel.MouseDown += MainOnMouseDown;
            Main.MouseLeave += MainOnMouseLeave;
            AddCommentButton.Click += AddCommentButtonOnClick;
            AddCommentPopup.LostFocus += AddCommentPopupOnLostFocus;
            Messenger.Default.Register<DistributeTaskStateColumnsMessage>(this, OnDistributeTaskStateColumnsMessage);
            var currentTaskStateColumnWidth = IocLocator.TaskBoardViewModel.CurrentOpenedTaskStateColumnWidth;
            OnDistributeTaskStateColumnsMessage(new DistributeTaskStateColumnsMessage
            {
                OpenedTaskStateColumnWidth = currentTaskStateColumnWidth
            });
        }

        private void OnDistributeTaskStateColumnsMessage(DistributeTaskStateColumnsMessage message)
        {
            var columnWidth = message.OpenedTaskStateColumnWidth;

            if (double.IsInfinity(columnWidth) ||
                double.IsNaN(columnWidth))
            {
                return;
            }

            if (columnWidth <= DefaultWidth)
            {
                Width = columnWidth - 10;
                return;
            }
            var tasksPerColumn = Math.Max(Math.Floor((columnWidth - 10) / DefaultWidth), 1);
            Width = Math.Floor((columnWidth - 10) / tasksPerColumn);
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