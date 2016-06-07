using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Task;

namespace GitTask.UI.MVVM.ViewModel.Merging
{
    public class TaskContentViewModel : ViewModelBase
    {
        public Task Task { get; }
        public ObservableCollection<string> Comments { get; }

        public bool AnyComments => Comments != null && Comments.Any();

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

        public TaskContentViewModel(Task task)
        {
            _isChosen = false;
            Task = task;
            if (task != null)
            {
                Comments = new ObservableCollection<string>(Task.Comments);
            }
        }
    }
}