using System.Windows.Input;
using GitTask.UI.MVVM.Locator;
using GitTask.UI.MVVM.ViewModel.Merging;

namespace GitTask.UI.MVVM.View.Merging
{
    public partial class TaskContentPartial
    {
        public TaskContentPartial()
        {
            InitializeComponent();
            Main.MouseDown += MainOnMouseDown;
        }

        private void MainOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var taskContent = DataContext as TaskContentViewModel;
            if (taskContent == null) return;

            taskContent.IsChosen = !taskContent.IsChosen;
            IocLocator.MergingViewModel.CurrentlyChosenTask = taskContent.Task;
        }
    }
}