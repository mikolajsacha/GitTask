using GitTask.UI.MVVM.ViewModel.History.TaskHistory;

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