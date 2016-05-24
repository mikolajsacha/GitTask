using System.Collections.ObjectModel;
using System.Collections.Specialized;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Project;

namespace GitTask.UI.MVVM.ViewModel.Elements
{
    public class SelectUsersViewModel : ViewModelBase
    {
        public string SelectionMode { get; }
        public ObservableCollection<ProjectMember> SelectedUsers { get; set; }
        public ProjectMember LastSelectedUser { get; private set; }

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
            SelectedUsers.CollectionChanged += OnSelectedUsersOnCollectionChanged;
        }

        private void OnSelectedUsersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e?.NewItems?.Count > 0)
            {
                LastSelectedUser = (ProjectMember)e.NewItems[e.NewItems.Count - 1];
            }
            AnyUserChosen = SelectedUsers != null && SelectedUsers.Count > 0;
        }
    }
}