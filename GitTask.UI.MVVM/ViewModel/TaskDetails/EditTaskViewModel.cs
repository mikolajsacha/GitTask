using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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

        public string Title => _task.Title;
        public TaskState TaskState { get; }
        public Brush TaskStateColor { get; }

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
            TaskState = taskStateQueryService.GetByKey(task.State);

            var brushConverter = new BrushConverter();
            TaskStateColor = (Brush)brushConverter.ConvertFromString(TaskState.Color);

            _taskQueryService = taskQueryService;
            _okCommand = new RelayCommand(OnOkClick);
            _addTaskStateCommand = new RelayCommand(OnAddTaskStateClick);
            _deleteCommand = new RelayCommand(OnDeleteClick);

            SelectUsersViewModel = new SelectUsersViewModel(true);
            SelectTaskPriorityViewModel = new SelectTaskPriorityViewModel();

            _content = _task.Content;
            foreach (var user in _task.AssignedMembers)
            {
                SelectUsersViewModel.SelectedUsers.Add(user);
            }
            SelectTaskPriorityViewModel.SelectedTaskPriority = _task.Priority;
            SelectTaskPriorityViewModel.PropertyChanged += SelectTaskPriorityViewModelOnPropertyChanged;
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
                State = _task.State,
                Comments = _task.Comments
            });
            await _taskQueryService.SaveChanges();
        }
    }
}