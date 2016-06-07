using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Task;

namespace GitTask.UI.MVVM.ViewModel.Merging
{
    public class TaskStatesCollectionViewModel : ViewModelBase
    {
        public ObservableCollection<TaskState> TaskStates { get; }

        private bool _isChosen;
        public bool IsChosen
        {
            get { return _isChosen; }
            set
            {
                _isChosen = value;
                RaisePropertyChanged();
            }
        }

        public TaskStatesCollectionViewModel(IEnumerable<TaskState> taskStates)
        {
            _isChosen = false;
            TaskStates = new ObservableCollection<TaskState>(taskStates);
        }
    }
}