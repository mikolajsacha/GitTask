using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Model.Task;

namespace GitTask.UI.MVVM.Design
{
    public class DesignTaskStateColumnViewModel : ViewModelBase // based on GitTask.UI.MVVM.ViewModel.TaskBoard.TaskStateColumnViewModel
    {

        public TaskState TaskState { get; }
        public ObservableCollection<DesignTaskDetailsViewModel> Tasks { get; }

        public ICommand ShowColumnCommand { get; }
        public ICommand HideColumnCommand { get; }

        public bool IsOpened { get; private set; }
        public bool IsHidden => !IsOpened;

        public bool CanMoveLeft => true;
        public bool CanMoveRight => true;

        public DesignTaskStateColumnViewModel()
            : this(new TaskState { Color = Brushes.Green, Name = "TO DO", Position = 0 })
        {
        }

        public DesignTaskStateColumnViewModel(TaskState taskState, bool isOpened = true)
        {
            ShowColumnCommand = new RelayCommand(OnShowColumnCommand);
            HideColumnCommand = new RelayCommand(OnHideColumnCommand);

            IsOpened = isOpened;
            TaskState = taskState;
            Tasks = new ObservableCollection<DesignTaskDetailsViewModel>()
            {
                new DesignTaskDetailsViewModel(),
                new DesignTaskDetailsViewModel(),
                new DesignTaskDetailsViewModel(),
                new DesignTaskDetailsViewModel()
            };
        }

        private void OnShowColumnCommand()
        {
            IsOpened = false;
        }

        private void OnHideColumnCommand()
        {
            IsOpened = false;
        }
    }
}
