using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GitTask.UI.MVVM.Messages;
using GitTask.UI.MVVM.ViewModel.Elements;

namespace GitTask.UI.MVVM.ViewModel.ProjectSettings
{
    public class SetCurrentUserViewModel : ViewModelBase
    {
        public SelectUsersViewModel SelectUsersViewModel { get; }

        private readonly RelayCommand _okCommand;
        public ICommand OkCommand => _okCommand;

        public SetCurrentUserViewModel()
        {
            _okCommand = new RelayCommand(OnOkClick);
            SelectUsersViewModel = new SelectUsersViewModel(false);
        }

        private void OnOkClick()
        {
            Messenger.Default.Send(new SetCurrentUserMessage { CurrentUser = SelectUsersViewModel.SelectedUsers.First() });
        }
    }
}
