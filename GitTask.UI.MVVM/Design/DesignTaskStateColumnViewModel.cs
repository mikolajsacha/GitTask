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

        public bool IsOpened = true;
        public bool IsHidden => !IsOpened;

        public DesignTaskStateColumnViewModel()
        {
            ShowColumnCommand = new RelayCommand(OnShowColumnCommand);
            HideColumnCommand = new RelayCommand(OnHideColumnCommand);

            TaskState = new TaskState
            {
                Color = Brushes.Green,
                Name = "To do",
                Position = 1
            };

            Tasks = new ObservableCollection<DesignTaskDetailsViewModel>()
            {
                //TODO
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
