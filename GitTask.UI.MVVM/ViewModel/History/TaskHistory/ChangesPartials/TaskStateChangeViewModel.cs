using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;
using GitTask.Storage.Exception;

namespace GitTask.UI.MVVM.ViewModel.History.TaskHistory.ChangesPartials
{
    public class TaskStateChangeViewModel : BaseChangeViewModel<string>
    {
        public TaskState OldTaskState { get; private set; }
        public TaskState NewTaskState { get; private set; }

        public TaskStateChangeViewModel(string oldValue, string newValue, IQueryService<TaskState> taskStateQueryService)
                                      : base(oldValue, newValue)
        {
            OldTaskState = null;
            NewTaskState = null;
            TryResolveTaskStates(taskStateQueryService);
        }

        private void TryResolveTaskStates(IQueryService<TaskState> taskStateQueryService)
        {
            try
            {
                OldTaskState = taskStateQueryService.GetByKey(OldValue);
                NewTaskState = taskStateQueryService.GetByKey(NewValue);
            }
            catch (KeyNotExistsException)
            {
                // one of task states must have been deleted. Use bare string names 
                OldTaskState = null;
                NewTaskState = null;
            }
        }
    }
}