using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.View.TaskDetails;

namespace GitTask.UI.MVVM.ViewModel.TaskDetails
{
    public class TaskDetailsViewModel : ViewModelBase
    {
        private readonly IQueryService<Task> _taskQueryService;
        private readonly IQueryService<TaskState> _taskStateQueryService;
        public Task Task { get; }

        private readonly RelayCommand _editTaskCommand;
        public ICommand EditTaskCommand => _editTaskCommand;

        public TaskDetailsViewModel(Task task, IQueryService<Task> taskQueryService, IQueryService<TaskState> taskStateQueryService)
        {
            Task = task;
            _taskQueryService = taskQueryService;
            _taskStateQueryService = taskStateQueryService;
            _editTaskCommand = new RelayCommand(OnEditTaskCommand);
        }

        private void OnEditTaskCommand()
        {
            var editTaskWindow = new EditTaskWindow(new EditTaskViewModel(Task, _taskQueryService, _taskStateQueryService));
            editTaskWindow.Show();
        }
    }
}