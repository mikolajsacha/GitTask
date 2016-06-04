using GitTask.Domain.Enum;
using GitTask.UI.MVVM.ViewModel.TaskHistory;

namespace GitTask.UI.MVVM.Design.TaskHistory
{
    public class DesignTaskPriorityChangeViewModel : TaskPriorityChangeViewModel
    {
        public DesignTaskPriorityChangeViewModel() : base(TaskPriority.Major, TaskPriority.Critical)
        {
           
        }
    }
}