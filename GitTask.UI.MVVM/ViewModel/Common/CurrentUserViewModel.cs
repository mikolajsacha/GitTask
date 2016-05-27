using System;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Project;

namespace GitTask.UI.MVVM.ViewModel.Common
{
    public class CurrentUserViewModel : ViewModelBase
    {
        private ProjectMember _currentUser;
        public ProjectMember CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                _currentUser = value;
                RaisePropertyChanged();
                CurrentUserSet?.Invoke(value);
            }
        }
        public event Action<ProjectMember> CurrentUserSet;
    }
}