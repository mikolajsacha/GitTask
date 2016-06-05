using GitTask.Domain.Enum;
using GitTask.UI.MVVM.ViewModel.History.TaskHistory.ChangesPartials;

namespace GitTask.UI.MVVM.Design.TaskHistory
{
    public class DesignTaskPriorityChangeViewModel : TaskPriorityChangeViewModel
    {
        public DesignTaskPriorityChangeViewModel() : base(TaskPriority.Major, TaskPriority.Critical)
        {
           
        }
    }
}