using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Enum;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.ViewModel.Elements;

namespace GitTask.UI.MVVM.ViewModel.TaskDetails
{
    public class AddTaskViewModel : ViewModelBase
    {
        private readonly IQueryService<Task> _taskQueryService;
        public SelectUsersViewModel SelectUsersViewModel { get; }
        public SelectTaskPriorityViewModel SelectTaskPriorityViewModel { get; }

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

        private TaskState _state;
        public TaskState State
        {
            get { return _state; }
            set
            {
                _state = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsOkButtonEnabled");
            }
        }

        public bool IsOkButtonEnabled =>
            !string.IsNullOrWhiteSpace(_content) &&
            !string.IsNullOrWhiteSpace(_title) &&
            _state != null && //TODO: dodac wybor stanu rowniez z mozliwoscia dodania nowego stanu
            SelectTaskPriorityViewModel.TaskPriorityChosen;

        private readonly RelayCommand _okCommand;
        public ICommand OkCommand => _okCommand;

        public AddTaskViewModel(IQueryService<Task> taskQueryService)
        {
            _taskQueryService = taskQueryService;
            _okCommand = new RelayCommand(OnOkClick);

            SelectUsersViewModel = new SelectUsersViewModel(true);
            SelectTaskPriorityViewModel = new SelectTaskPriorityViewModel();
            SelectTaskPriorityViewModel.PropertyChanged += SelectTaskPriorityViewModelOnPropertyChanged;
        }

        private void SelectTaskPriorityViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "TaskPriorityChosen")
            {
                RaisePropertyChanged("IsOkButtonEnabled");
            }
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
                State = _state.Name

            });
            await _taskQueryService.SaveChanges();
        }
    }
}