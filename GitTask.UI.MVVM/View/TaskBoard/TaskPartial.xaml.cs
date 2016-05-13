﻿using System.Windows;
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
            Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Main.MouseDown -= MainOnMouseDown;
        }

        private void MainOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var taskDetails = DataContext as TaskDetailsViewModel;
            if (taskDetails == null) return;

            var taskDetailsWindow = new TaskDetailsWindow(taskDetails);
            taskDetailsWindow.ShowDialog();
        }
    }
}