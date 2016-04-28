using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;
using Microsoft.Practices.ServiceLocation;

namespace GitTask.UI.MVVM.ViewModel.TaskDetails
{
    public class TaskDetailsViewModel : ViewModelBase
    {
        private IQueryService<Task> _taskQueryService;

        public Task Task { get; }

        public TaskDetailsViewModel(Task task)
        {
            Task = task;
            _taskQueryService = ServiceLocator.Current.GetInstance<IQueryService<Task>>();
        }
    }
}