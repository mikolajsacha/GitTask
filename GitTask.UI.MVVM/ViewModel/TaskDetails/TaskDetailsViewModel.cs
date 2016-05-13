using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.TaskDetails
{
    public class TaskDetailsViewModel : ViewModelBase
    {
        private IQueryService<Task> _taskQueryService;
        public Task Task { get; }

        public TaskDetailsViewModel(Task task, IQueryService<Task> taskQueryService)
        {
            Task = task;
            _taskQueryService = taskQueryService;
        }
    }
}