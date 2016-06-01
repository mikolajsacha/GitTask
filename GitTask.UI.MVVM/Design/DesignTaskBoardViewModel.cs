using System.Collections.ObjectModel;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Task;

namespace GitTask.UI.MVVM.Design
{
    public class DesignTaskBoardViewModel : ViewModelBase // based on GitTask.UI.MVVM.ViewModel.TaskBoard.TaskBoardViewModel
    {
        public ObservableCollection<DesignTaskStateColumnViewModel> TaskStateColumns { get; }
        public bool IsLoading => false;

        public int OpenedColumnsCount => 4;
        public int HiddenColumnsCount => 1;

        public DesignTaskBoardViewModel()
        {
            TaskStateColumns = new ObservableCollection<DesignTaskStateColumnViewModel>
            {
                new DesignTaskStateColumnViewModel(),
                new DesignTaskStateColumnViewModel(new TaskState {Color = Brushes.Red, Name = "IN PROGRESS", Position = 1}),
                new DesignTaskStateColumnViewModel(new TaskState {Color = Brushes.Blue, Name = "TO BE TESTED", Position = 2}),
                new DesignTaskStateColumnViewModel(new TaskState {Color = Brushes.Orange, Name = "DONE", Position = 3}),
                new DesignTaskStateColumnViewModel(new TaskState {Color = Brushes.Gray, Name = "CLOSED", Position = 4}, false)
            };
        }
    }
}