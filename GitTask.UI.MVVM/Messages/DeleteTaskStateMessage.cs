using GitTask.Domain.Model.Task;

namespace GitTask.UI.MVVM.Messages
{
    public class DeleteTaskStateMessage
    {
        public TaskState TaskState { get; set; }
    }
}
