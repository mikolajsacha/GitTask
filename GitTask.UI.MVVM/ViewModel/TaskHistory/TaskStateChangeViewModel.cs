using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Task;

namespace GitTask.UI.MVVM.ViewModel.TaskHistory
{
    public class TaskStateChangeViewModel : BaseChangeViewModel<TaskState>
    {
        public TaskStateChangeViewModel(TaskState oldValue, TaskState newValue)
                                      : base(oldValue, newValue)
        {
           
        }
    }
}