using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GitTask.Repository.Model;
using GitTask.UI.MVVM.Messages;

namespace GitTask.UI.MVVM.ViewModel.ProjectSettings
{
    public class SetCurrentUserViewModel : ViewModelBase
    {
        private ProjectMember _selectedUser;
        public ProjectMember SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsOkButtonEnabled");
            }
        }

        public bool IsOkButtonEnabled => _selectedUser != null;

        private readonly RelayCommand _okCommand;
        public ICommand OkCommand => _okCommand;

        public SetCurrentUserViewModel()
        {
            _okCommand = new RelayCommand(OnOkClick);
        }

        private void OnOkClick()
        {
            Messenger.Default.Send(new SetCurrentUserMessage { CurrentUser = _selectedUser });
        }
    }
}
