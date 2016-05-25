using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.Messages;
using GitTask.UI.MVVM.ViewModel.TaskDetails;

namespace GitTask.UI.MVVM.ViewModel.TaskBoard
{
    public class TaskStateColumnViewModel : ViewModelBase
    {
        private readonly IQueryService<TaskState> _taskStateQueryService;

        public TaskState TaskState { get; }
        public ObservableCollection<TaskDetailsViewModel> Tasks { get; }

        private readonly RelayCommand _showColumnCommand;
        public ICommand ShowColumnCommand => _showColumnCommand;

        private readonly RelayCommand _hideColumnCommand;
        public ICommand HideColumnCommand => _hideColumnCommand;

        private readonly RelayCommand _moveColumnRightCommand;
        public ICommand MoveColumnRightCommand => _moveColumnRightCommand;

        private readonly RelayCommand _moveColumnLeftCommand;
        public ICommand MoveColumnLeftCommand => _moveColumnLeftCommand;

        private readonly RelayCommand _deleteTaskStateCommand;
        public ICommand DeleteTaskStateCommand => _deleteTaskStateCommand;

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
        public bool CanBeDeleted => Tasks == null || Tasks.Count == 0;
        public bool CanMoveLeft { get; }
        public bool CanMoveRight { get; }

        public TaskStateColumnViewModel(TaskState taskState,
                                        IQueryService<TaskState> taskStateQueryService,
                                        bool isOpened, bool canMoveLeft, bool canMoveRight)
        {
            _taskStateQueryService = taskStateQueryService;
            _isOpened = isOpened;
            TaskState = taskState;

            _showColumnCommand = new RelayCommand(OnShowColumnCommand);
            _hideColumnCommand = new RelayCommand(OnHideColumnCommand);
            _moveColumnLeftCommand = new RelayCommand(OnMoveColumnLeftCommand);
            _moveColumnRightCommand = new RelayCommand(OnMoveColumRightCommand);
            _deleteTaskStateCommand = new RelayCommand(OnDeleteTaskStateCommand);

            CanMoveLeft = canMoveLeft;
            CanMoveRight = canMoveRight;

            Tasks = new ObservableCollection<TaskDetailsViewModel>();
            Tasks.CollectionChanged += (sender, args) => RaisePropertyChanged("CanBeDeleted");
        }

        private void OnHideColumnCommand()
        {
            IsOpened = false;
        }

        private void OnShowColumnCommand()
        {
            IsOpened = true;
        }

        private void OnDeleteTaskStateCommand()
        {
            Messenger.Default.Send(new DeleteTaskStateMessage { TaskState = TaskState });
        }

        private async void OnMoveColumnLeftCommand()
        {
            var leftTaskStateQuery =
                _taskStateQueryService.GetAll()
                    .Where(taskState => taskState.Position < TaskState.Position)
                    .OrderBy(taskState => taskState.Position);

            var taskStateQueryResult = leftTaskStateQuery.ToList();

            if (!taskStateQueryResult.Any()) return;

            var leftTaskState = taskStateQueryResult.Last();

            var temp = leftTaskState.Position;
            leftTaskState.Position = TaskState.Position;
            TaskState.Position = temp;

            _taskStateQueryService.Update(TaskState);
            _taskStateQueryService.Update(leftTaskState);
            await _taskStateQueryService.SaveChanges();
        }

        private async void OnMoveColumRightCommand()
        {
            var rightTaskStateQueryResult =
                _taskStateQueryService.GetAll()
                    .Where(taskState => taskState.Position > TaskState.Position)
                    .OrderBy(taskState => taskState.Position);

            var taskStateQueryResult = rightTaskStateQueryResult.ToList();

            if (!taskStateQueryResult.Any()) return;

            var rightTaskState = taskStateQueryResult.First();

            var temp = rightTaskState.Position;
            rightTaskState.Position = TaskState.Position;
            TaskState.Position = temp;

            _taskStateQueryService.Update(TaskState);
            _taskStateQueryService.Update(rightTaskState);
            await _taskStateQueryService.SaveChanges();
        }
    }
}
