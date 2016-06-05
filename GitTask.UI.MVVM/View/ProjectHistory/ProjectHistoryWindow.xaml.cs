using GitTask.UI.MVVM.ViewModel.History.ProjectHistory;

namespace GitTask.UI.MVVM.View.ProjectHistory
{
    public partial class ProjectHistoryWindow
    {
        public ProjectHistoryWindow(ProjectHistoryViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}