using System.Windows.Controls;
using System.Windows.Input;
using GitTask.UI.MVVM.Locator;
using GitTask.UI.MVVM.ViewModel.Merging;

namespace GitTask.UI.MVVM.View.Merging
{
    public partial class TaskStatePartial
    {
        public TaskStatePartial()
        {
            InitializeComponent();
            MouseDown += MainOnMouseDown;
        }

        private void MainOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var taskStateContent = DataContext as TaskStatesCollectionViewModel;
            if (taskStateContent == null) return;

            taskStateContent.IsChosen = !taskStateContent.IsChosen;
            if (taskStateContent.IsChosen)
            {
                IocLocator.MergingViewModel.CurrentlyChosenTaskStates = taskStateContent.TaskStates;
            }
        }

        private void ScrollViewer_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollviewer = sender as ScrollViewer;
            if (scrollviewer == null) return;
            if (e.Delta > 0)
                scrollviewer.LineLeft();
            else
                scrollviewer.LineRight();
            e.Handled = true;
        }
    }
}