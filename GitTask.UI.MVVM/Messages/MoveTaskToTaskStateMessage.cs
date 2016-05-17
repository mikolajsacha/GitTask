namespace GitTask.UI.MVVM.Messages
{
    public class MoveTaskToTaskStateMessage
    {
        public string TaskName { get; set; }
        public string NewTaskStateName { get; set; }
    }
}
