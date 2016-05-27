using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.UI.MVVM.ViewModel.Common;
using GitTask.UI.MVVM.ViewModel.Elements;

namespace GitTask.UI.MVVM.ViewModel.ProjectSettings
{
    public class SetCurrentUserViewModel : ViewModelBase
    {
        private readonly CurrentUserViewModel _currentUserViewModel;
        public SelectUsersViewModel SelectUsersViewModel { get; }

        private readonly RelayCommand _okCommand;
        public ICommand OkCommand => _okCommand;

        public SetCurrentUserViewModel(CurrentUserViewModel currentUserViewModel)
        {
            _currentUserViewModel = currentUserViewModel;
            _okCommand = new RelayCommand(OnOkClick);
            SelectUsersViewModel = new SelectUsersViewModel(false);
        }

        private void OnOkClick()
        {
            _currentUserViewModel.CurrentUser = SelectUsersViewModel.LastSelectedUser;
        }
    }
}
