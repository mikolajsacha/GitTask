using System.ComponentModel;
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
    public class AddTaskViewModel : ViewModelBase
    {
        private readonly IQueryService<Task> _taskQueryService;

        public SelectUsersViewModel SelectUsersViewModel { get; }
        public SelectTaskPriorityViewModel SelectTaskPriorityViewModel { get; }
        public SelectTaskStateViewModel SelectTaskStateViewModel { get; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsOkButtonEnabled");
            }
        }

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
            !string.IsNullOrWhiteSpace(_title) &&
            SelectTaskStateViewModel.TaskStateChosen &&
            SelectTaskPriorityViewModel.TaskPriorityChosen;

        private readonly RelayCommand _okCommand;
        public ICommand OkCommand => _okCommand;

        private readonly RelayCommand _addTaskStateCommand;
        public ICommand AddTaskStateCommand => _addTaskStateCommand;

        public AddTaskViewModel(IQueryService<Task> taskQueryService, IQueryService<TaskState> taskStateQueryService)
        {
            _taskQueryService = taskQueryService;
            _okCommand = new RelayCommand(OnOkClick);
            _addTaskStateCommand = new RelayCommand(OnAddTaskStateClick);

            SelectUsersViewModel = new SelectUsersViewModel(true);
            SelectTaskStateViewModel = new SelectTaskStateViewModel(taskStateQueryService);
            SelectTaskStateViewModel.PropertyChanged += SelectTaskStateViewModelOnPropertyChanged;
            SelectTaskPriorityViewModel = new SelectTaskPriorityViewModel();
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

        private void OnAddTaskStateClick()
        {
            var addTaskStateWindow = new AddTaskStateWindow();
            addTaskStateWindow.ShowDialog();
        }

        private async void OnOkClick()
        {
            _taskQueryService.AddNew(new Task
            {
                Title = _title,
                Content = _content,
                AssignedMembers = SelectUsersViewModel.SelectedUsers,
                // ReSharper disable once PossibleInvalidOperationException
                Priority = (TaskPriority)SelectTaskPriorityViewModel.SelectedTaskPriority,
                State = SelectTaskStateViewModel.SelectedTaskState.Name

            });
            await _taskQueryService.SaveChanges();
        }
    }
}