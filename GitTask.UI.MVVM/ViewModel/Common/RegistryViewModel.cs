﻿using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.Model;
using Microsoft.Win32;

namespace GitTask.UI.MVVM.ViewModel.Common
{
    public class RegistryViewModel : ViewModelBase
    {
        private const string BaseSubKeyName = "GitTask";
        private RegistryKey _baseRegistryKey;

        private readonly IProjectPathsService _projectPathsService;
        private readonly IRepositoryService _repositoryService;
        private readonly CurrentUserViewModel _currentUserViewModel;

        private RegistryProjectInformation _currentProject;
        public RegistryProjectInformation CurrentProject
        {
            get { return _currentProject; }
            set
            {
                _currentProject = value;
                RaisePropertyChanged();
                try
                {
                    Task.Run(() => TrySaveCurrentProjectToRegistry());
                }
                catch (Exception)
                {
                    // Exception. We can't use registry
                }
            }
        }

        public RegistryViewModel(IProjectPathsService projectPathsService,
                                 IRepositoryService repositoryService,
                                 CurrentUserViewModel currentUserViewModel)
        {
            _projectPathsService = projectPathsService;
            _repositoryService = repositoryService;
            _currentUserViewModel = currentUserViewModel;
        }

        public void InitializeRegistry()
        {
            try
            {
                using (var registryKey = Registry.CurrentUser.OpenSubKey(BaseSubKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    if (registryKey != null)
                    {
                        CreateBaseRegistryKeyIfNotExists();
                        ScanRegistry();
                    }
                }
                if (CurrentProject != null && _repositoryService.RepositoryExists(CurrentProject.ProjectPath))
                {
                    _projectPathsService.BaseProjectPath = CurrentProject.ProjectPath;
                    _currentUserViewModel.CurrentUser = CurrentProject.CurrentUser;
                }
                _projectPathsService.ProjectPathChanged += ProjectPathsServiceOnProjectPathChanged;
                _currentUserViewModel.CurrentUserSet += CurrentUserViewModelOnCurrentUserSet;
            }
            catch (Exception)
            {
                // Exception. We can't use registry
            }
        }

        private void CurrentUserViewModelOnCurrentUserSet(ProjectMember currentUser)
        {
            CurrentProject = new RegistryProjectInformation
            {
                ProjectPath = CurrentProject.ProjectPath,
                CurrentUser = currentUser
            };
        }

        private void ProjectPathsServiceOnProjectPathChanged()
        {
            CurrentProject = new RegistryProjectInformation
            {
                ProjectPath = _projectPathsService.BaseProjectPath,
                CurrentUser = null
            };
        }

        private void TrySaveCurrentProjectToRegistry()
        {
            CreateBaseRegistryKeyIfNotExists();
            _baseRegistryKey.SetValue("CurrentProjectPath", CurrentProject.ProjectPath);
            if (CurrentProject.CurrentUser == null) return;
            _baseRegistryKey.SetValue("CurrentProjectUserName", CurrentProject.CurrentUser.Name);
            _baseRegistryKey.SetValue("CurrentProjectUserEmail", CurrentProject.CurrentUser.Email);
        }

        private void CreateBaseRegistryKeyIfNotExists()
        {
            _baseRegistryKey = Registry.CurrentUser.OpenSubKey(BaseSubKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree) ??
                          Registry.CurrentUser.CreateSubKey(BaseSubKeyName);
        }

        private void ScanRegistry()
        {
            var projectPath = (string)_baseRegistryKey.GetValue("CurrentProjectPath", null);
            var userName = (string)_baseRegistryKey.GetValue("CurrentProjectUserName", null);
            var userEmail = (string)_baseRegistryKey.GetValue("CurrentProjectUserEmail", null);
            if (projectPath == null || userName == null || userEmail == null) return;
            CurrentProject = new RegistryProjectInformation
            {
                CurrentUser = new ProjectMember(userName, userEmail),
                ProjectPath = projectPath
            };
        }
    }
}