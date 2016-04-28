using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.Main
{
    public class TaskStateColumnViewModel : ViewModelBase
    {
        private readonly IQueryService<Task> _taskQueryService;

        public TaskState TaskState { get; }
        public ObservableCollection<Task> Tasks { get; }

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
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged("IsHidden");
            }
        }

        public bool IsHidden => !_isOpened;

        public TaskStateColumnViewModel(TaskState taskState, IQueryService<Task> taskQueryService, bool isOpened)
        {
            _showColumnCommand = new RelayCommand(OnShowColumnCommand);
            _hideColumnCommand = new RelayCommand(OnHideColumnCommand);

            _isOpened = isOpened;
            TaskState = taskState;
            _taskQueryService = taskQueryService;
            Tasks = new ObservableCollection<Task>();
            LoadTasks();
        }

        public void LoadTasks()
        {
            Tasks.Clear();
            foreach (var task in _taskQueryService.GetByProperty("State", TaskState.Name).OrderBy(x => x.Priority))
            {
                Tasks.Add(task);
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
