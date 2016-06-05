using GitTask.Domain.Model.Task;
using GitTask.UI.MVVM.ViewModel.History;

namespace GitTask.UI.MVVM.Design.TaskHistory
{
    public class DesignTaskStateChangeViewModel : BaseChangeViewModel<string>
    {
        public TaskState OldTaskState { get; set; }
        public TaskState NewTaskState { get; set; }

        public DesignTaskStateChangeViewModel() : base("TO DO", "IN PROGRESS") { }
    }
}