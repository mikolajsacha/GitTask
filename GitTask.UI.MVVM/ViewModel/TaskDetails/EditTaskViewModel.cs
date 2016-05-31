using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Enum;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.View.ProjectSettings;
using GitTask.UI.MVVM.ViewModel.Elements;

namespace GitTask.UI.MVVM.ViewModel.TaskDetails
{
    public class EditTaskViewModel : ViewModelBase
    {
        private readonly Task _task;
        private readonly IQueryService<Task> _taskQueryService;

        public SelectUsersViewModel SelectUsersViewModel { get; }
        public SelectTaskPriorityViewModel SelectTaskPriorityViewModel { get; }
        public SelectTaskStateViewModel SelectTaskStateViewModel { get; }

        public string Title => _task.Title;

        private string _content;
        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsOkButtonEnabled");
            }
        }

        public bool IsOkButtonEnabled =>
            !string.IsNullOrWhiteSpace(_content) &&
            SelectTaskStateViewModel.TaskStateChosen &&
            SelectTaskPriorityViewModel.TaskPriorityChosen;

        private readonly RelayCommand _okCommand;
        public ICommand OkCommand => _okCommand;

        private readonly RelayCommand _addTaskStateCommand;
        public ICommand AddTaskStateCommand => _addTaskStateCommand;

        private readonly RelayCommand _deleteCommand;
        public ICommand DeleteCommand => _deleteCommand;

        public EditTaskViewModel(Task task, IQueryService<Task> taskQueryService, IQueryService<TaskState> taskStateQueryService)
        {
            _task = task;

            _taskQueryService = taskQueryService;
            _okCommand = new RelayCommand(OnOkClick);
            _addTaskStateCommand = new RelayCommand(OnAddTaskStateClick);
            _deleteCommand = new RelayCommand(OnDeleteClick);

            SelectUsersViewModel = new SelectUsersViewModel(true);
            SelectTaskStateViewModel = new SelectTaskStateViewModel(taskStateQueryService);
            SelectTaskPriorityViewModel = new SelectTaskPriorityViewModel();

            _content = _task.Content;
            foreach (var user in _task.AssignedMembers)
            {
                SelectUsersViewModel.SelectedUsers.Add(user);
            }
            SelectTaskStateViewModel.SelectedTaskState = taskStateQueryService.GetByKey(_task.State);
            SelectTaskPriorityViewModel.SelectedTaskPriority = _task.Priority;

            SelectTaskStateViewModel.PropertyChanged += SelectTaskStateViewModelOnPropertyChanged;
            SelectTaskPriorityViewModel.PropertyChanged += SelectTaskPriorityViewModelOnPropertyChanged;
        }

        private void SelectTaskStateViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "TaskStateChosen")
            {
                RaisePropertyChanged("IsOkButtonEnabled");
            }
        }

        private void SelectTaskPriorityViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "TaskPriorityChosen")
            {
                RaisePropertyChanged("IsOkButtonEnabled");
            }
        }

        private async void OnDeleteClick()
        {
            _taskQueryService.Delete(_task.Title);
            await _taskQueryService.SaveChanges();
        }

        private void OnAddTaskStateClick()
        {
            var addTaskStateWindow = new AddTaskStateWindow { Owner = Application.Current.MainWindow };
            addTaskStateWindow.ShowDialog();
        }

        private async void OnOkClick()
        {
            _taskQueryService.Update(new Task
            {
                Title = Title,
                Content = _content,
                AssignedMembers = SelectUsersViewModel.SelectedUsers,
                // ReSharper disable once PossibleInvalidOperationException
                Priority = (TaskPriority)SelectTaskPriorityViewModel.SelectedTaskPriority,
                State = SelectTaskStateViewModel.SelectedTaskState.Name,
                Comments = _task.Comments
            });
            await _taskQueryService.SaveChanges();
        }
    }
}