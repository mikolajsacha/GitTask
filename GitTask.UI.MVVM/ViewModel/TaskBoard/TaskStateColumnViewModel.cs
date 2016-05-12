using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.ViewModel.TaskDetails;

namespace GitTask.UI.MVVM.ViewModel.TaskBoard
{
    public class TaskStateColumnViewModel : ViewModelBase
    {
        private readonly IQueryService<Task> _taskQueryService;

        public TaskState TaskState { get; }
        public ObservableCollection<TaskDetailsViewModel> Tasks { get; }

        private readonly RelayCommand _showColumnCommand;
        public ICommand ShowColumnCommand => _showColumnCommand;

        private readonly RelayCommand _hideColumnCommand;
        public ICommand HideColumnCommand => _hideColumnCommand;

        private bool _isOpened;
        public bool IsOpened
        {
            get { return _isOpened; }
            set
            {
                _isOpened = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsHidden");
            }
        }

        public bool IsHidden => !_isOpened;

        public TaskStateColumnViewModel(TaskState taskState,
                                        IQueryService<Task> taskQueryService,
                                        bool isOpened)
        {
            _taskQueryService = taskQueryService;
            _isOpened = isOpened;
            TaskState = taskState;

            _showColumnCommand = new RelayCommand(OnShowColumnCommand);
            _hideColumnCommand = new RelayCommand(OnHideColumnCommand);

            Tasks = new ObservableCollection<TaskDetailsViewModel>();
            LoadTasks();
        }

        public void LoadTasks()
        {
            Tasks.Clear();
            foreach (var task in _taskQueryService.GetByProperty("State", TaskState.Name).OrderBy(x => x.Priority))
            {
                Tasks.Add(new TaskDetailsViewModel(task, _taskQueryService));
            }
        }

        private void OnHideColumnCommand()
        {
            IsOpened = false;
        }

        private void OnShowColumnCommand()
        {
            IsOpened = true;
        }
    }
}
