using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.ViewModel.Common;

namespace GitTask.UI.MVVM.ViewModel.ActionBar
{
    public class FiltersViewModel : ViewModelBase
    {
        private ProjectMember _currentUser;
        private readonly ProjectMembersSetsViewModel _projectMembersSetsViewModel;

        private bool _currentUserFilter;
        public bool CurrentUserFilter
        {
            get { return _currentUserFilter; }
            set
            {
                _currentUserFilter = value;
                if (value && _currentUser != null)
                {
                    Task.Run(async () =>
                    {
                        FilteredUsers = await _projectMembersSetsViewModel.Resolve(_currentUser);
                        FiltersUpdated?.Invoke();
                    });
                }
                else
                {
                    FiltersUpdated?.Invoke();
                }
                RaisePropertyChanged();
            }
        }

        private bool _unassignedFilter;
        public bool UnassignedFilter
        {
            get { return _unassignedFilter; }
            set
            {
                _unassignedFilter = value;
                FiltersUpdated?.Invoke();
                RaisePropertyChanged();
            }
        }

        public event Action FiltersUpdated;

        private bool _areFiltersEnabled;
        public bool AreFiltersEnabled
        {
            get { return _areFiltersEnabled; }
            set
            {
                _areFiltersEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsCurrentUserFilterEnabled => _areFiltersEnabled && _currentUser != null;

        public IEnumerable<ProjectMember> FilteredUsers { get; private set; }

        public FiltersViewModel(IProjectPathsReadonlyService projectPathsService,
                                ProjectMembersSetsViewModel projectMembersSetsViewModel,
                                CurrentUserViewModel currentUserViewModel)
        {
            _projectMembersSetsViewModel = projectMembersSetsViewModel;
            _areFiltersEnabled = projectPathsService.IsProjectPathChosen;
            projectPathsService.ProjectPathChanged += OnProjectPathChanged;
            currentUserViewModel.CurrentUserSet += CurrentUserViewModelOnCurrentUserSet;
            _currentUser = currentUserViewModel.CurrentUser;
        }

        private async void CurrentUserViewModelOnCurrentUserSet(ProjectMember currentUser)
        {
            if (_currentUserFilter && _currentUser != null)
            {
                await Task.Run(async () =>
                {
                    FilteredUsers = await _projectMembersSetsViewModel.Resolve(_currentUser);
                    FiltersUpdated?.Invoke();
                });
            }
            _currentUser = currentUser;
            RaisePropertyChanged("IsCurrentUserFilterEnabled");
        }

        private void OnProjectPathChanged()
        {
            AreFiltersEnabled = true;
            RaisePropertyChanged("IsCurrentUserFilterEnabled");
        }
    }
}