using System.Collections.ObjectModel;
using System.Windows.Media;
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
                new TaskState {Color = Brushes.Yellow, Name = "TO DO", Position = 0},
                new TaskState {Color = Brushes.Green, Name = "IN PROGRESS", Position = 1}
            };
        }
    }
}