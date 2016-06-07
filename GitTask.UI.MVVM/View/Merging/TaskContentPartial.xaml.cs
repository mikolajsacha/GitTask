using System.Windows.Input;
using GitTask.UI.MVVM.ViewModel.Merging;

namespace GitTask.UI.MVVM.View.Merging
{
    public partial class TaskContentPartial
    {
        public TaskContentPartial()
        {
            InitializeComponent();
            ContentGrid.MouseDown += MainOnMouseDown;
            CommentsPanel.MouseDown += MainOnMouseDown;
        }

        private void MainOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var taskContent = DataContext as TaskContentViewModel;
            if (taskContent == null) return;

            taskContent.IsChosen = !taskContent.IsChosen;
        }
    }
}