using GitTask.Domain.Enum;

namespace GitTask.UI.MVVM.ViewModel.History.TaskHistory.ChangesPartials
{
    public class TaskPriorityChangeViewModel : BaseChangeViewModel<TaskPriority>
    {
        public TaskPriorityChangeViewModel(TaskPriority oldValue, TaskPriority newValue)
                                         : base(oldValue, newValue)
        {
           
        }
    }
}