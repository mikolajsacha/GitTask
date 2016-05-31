using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GitTask.Domain.Model.Project;
using GitTask.UI.MVVM.Messages;

namespace GitTask.UI.MVVM.ViewModel.Elements
{
    public class SelectUsersViewModel : ViewModelBase
    {
        public string SelectionMode { get; }
        public ObservableCollection<ProjectMember> SelectedUsers { get; set; }
        public ProjectMember LastSelectedUser { get; private set; }

        private bool _anyUserChosen;
        private string _addedUserName;
        private string _addedUserEmail;

        public bool AnyUserChosen
        {
            get { return _anyUserChosen; }
            set
            {
                _anyUserChosen = value;
                RaisePropertyChanged();
            }
        }

        public string AddedUserName
        {
            get { return _addedUserName; }
            set
            {
                _addedUserName = value;
                RaisePropertyChanged();
                RaisePropertyChanged("AddUserButtonEnabled");
            }
        }

        public string AddedUserEmail
        {
            get { return _addedUserEmail; }
            set
            {
                _addedUserEmail = value;
                RaisePropertyChanged();
                RaisePropertyChanged("AddUserButtonEnabled");
            }
        }

        public bool AddUserButtonEnabled => !(string.IsNullOrWhiteSpace(AddedUserEmail) || string.IsNullOrWhiteSpace(AddedUserEmail));

        private readonly RelayCommand _addUserCommand;
        public ICommand AddUserCommand => _addUserCommand;

        public SelectUsersViewModel(bool isMultipleSelection)
        {
            SelectionMode = isMultipleSelection ? "Multiple" : "Single";

            _anyUserChosen = false;
            SelectedUsers = new ObservableCollection<ProjectMember>();
            SelectedUsers.CollectionChanged += OnSelectedUsersOnCollectionChanged;
            _addUserCommand = new RelayCommand(OnAddUserCommand);
        }

        private void OnSelectedUsersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e?.NewItems?.Count > 0)
            {
                LastSelectedUser = (ProjectMember)e.NewItems[e.NewItems.Count - 1];
            }
            AnyUserChosen = SelectedUsers != null && SelectedUsers.Count > 0;
        }

        private void OnAddUserCommand()
        {
            Messenger.Default.Send(new AddUserMessage { UserToBeAdded = new ProjectMember(AddedUserName, AddedUserEmail) });
        }
    }
}