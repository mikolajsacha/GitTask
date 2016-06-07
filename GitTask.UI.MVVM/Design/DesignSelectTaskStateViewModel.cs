using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Task;

namespace GitTask.UI.MVVM.Design
{
    public class DesignSelectTaskStateViewModel : ViewModelBase // based on GitTask.UI.MVVM.ViewModel.Element.SelectTaskStateViewModel
    {
        private TaskState _selectedTaskState;
        public TaskState SelectedTaskState
        {
            get { return _selectedTaskState; }
            set
            {
                _selectedTaskState = value;
                RaisePropertyChanged();
                RaisePropertyChanged("TaskStateChosen");
            }
        }

        public ObservableCollection<TaskState> AllTaskStates { get; }
        public bool TaskStateChosen => _selectedTaskState != null;

        public DesignSelectTaskStateViewModel()
        {
            AllTaskStates = new ObservableCollection<TaskState>
            {
                new TaskState {Color = "#FFFFFF00", Name = "TO DO", Position = 0},
                new TaskState {Color = "#FF00FF00", Name = "IN PROGRESS", Position = 1}
            };
        }
    }
}