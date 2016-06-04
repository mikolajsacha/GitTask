using GitTask.UI.MVVM.ViewModel.TaskHistory;

namespace GitTask.UI.MVVM.View.TaskHistory
{
    public partial class TaskHistoryWindow
    {
        public TaskHistoryWindow(TaskHistoryViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}