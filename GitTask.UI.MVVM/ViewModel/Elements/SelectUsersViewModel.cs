using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GitTask.Repository.Model;

namespace GitTask.UI.MVVM.ViewModel.Elements
{
    public class SelectUsersViewModel : ViewModelBase
    {
        public string SelectionMode { get; }
        public ObservableCollection<ProjectMember> SelectedUsers { get; set;  }

        private bool _anyUserChosen;
        public bool AnyUserChosen
        {
            get { return _anyUserChosen; }
            set
            {
                _anyUserChosen = value;
                RaisePropertyChanged();
            }
        }

        public SelectUsersViewModel(bool isMultipleSelection)
        {
            SelectionMode = isMultipleSelection ? "Multiple" : "Single";

            _anyUserChosen = false;
            SelectedUsers = new ObservableCollection<ProjectMember>();
            SelectedUsers.CollectionChanged += delegate { AnyUserChosen = SelectedUsers != null && SelectedUsers.Count > 0; };
        }
    }
}