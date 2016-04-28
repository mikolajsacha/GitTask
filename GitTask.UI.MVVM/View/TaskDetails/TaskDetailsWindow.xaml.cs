using GitTask.UI.MVVM.ViewModel.TaskDetails;

namespace GitTask.UI.MVVM.View.TaskDetails
{
    public partial class TaskDetailsWindow
    {
        public TaskDetailsWindow(TaskDetailsViewModel dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
        }
    }
}