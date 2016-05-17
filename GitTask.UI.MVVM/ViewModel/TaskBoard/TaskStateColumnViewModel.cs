using System.Collections.Generic;
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

            CanMoveLeft = canMoveLeft;
            CanMoveRight = canMoveRight;

            Tasks = new ObservableCollection<TaskDetailsViewModel>();
        }

        private void OnHideColumnCommand()
        {
            IsOpened = false;
        }

        private void OnShowColumnCommand()
        {
            IsOpened = true;
        }

        private async void OnMoveColumnLeftCommand()
        {
            if (TaskState.Position == 0) return;

            var leftTaskState = _taskStateQueryService.GetByProperty("Position", TaskState.Position - 1).First();

            TaskState.Position--;
            leftTaskState.Position++;

            _taskStateQueryService.Update(TaskState);
            _taskStateQueryService.Update(leftTaskState);
            await _taskStateQueryService.SaveChanges();
        }

        private async void OnMoveColumRightCommand()
        {
            var rightTaskStateQueryResult = _taskStateQueryService.GetByProperty("Position", TaskState.Position - 1);
            var taskStateQueryResult = rightTaskStateQueryResult as IList<TaskState> ?? rightTaskStateQueryResult.ToList();

            if (!taskStateQueryResult.Any()) return;

            var rightTaskState = taskStateQueryResult.First();

            TaskState.Position--;
            rightTaskState.Position++;

            _taskStateQueryService.Update(TaskState);
            _taskStateQueryService.Update(rightTaskState);
            await _taskStateQueryService.SaveChanges();
        }
    }
}
