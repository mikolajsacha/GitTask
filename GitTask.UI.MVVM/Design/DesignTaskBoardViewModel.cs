using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace GitTask.UI.MVVM.Design
{
    public class DesignTaskBoardViewModel : ViewModelBase // based on GitTask.UI.MVVM.ViewModel.TaskBoard.TaskBoardViewModel
    {
        public ObservableCollection<DesignTaskStateColumnViewModel> TaskStateColumns { get; }

        public int OpenedColumnsCount => 4;
        public int HiddenColumnsCount => 1;
       
        public DesignTaskBoardViewModel()
        {
            TaskStateColumns = new ObservableCollection<DesignTaskStateColumnViewModel>
            {
                //TODO
            };
        }
    }
}