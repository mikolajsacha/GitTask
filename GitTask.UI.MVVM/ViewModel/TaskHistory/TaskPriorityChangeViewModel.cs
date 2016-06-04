using GalaSoft.MvvmLight;
using GitTask.Domain.Enum;

namespace GitTask.UI.MVVM.ViewModel.TaskHistory
{
    public class TaskPriorityChangeViewModel : BaseChangeViewModel<TaskPriority>
    {
        public TaskPriorityChangeViewModel(TaskPriority oldValue, TaskPriority newValue)
                                         : base(oldValue, newValue)
        {
           
        }
    }
}