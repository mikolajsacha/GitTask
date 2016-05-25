using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.Messages;
using GitTask.UI.MVVM.Model;
using Microsoft.Win32;

namespace GitTask.UI.MVVM.ViewModel.Common
{
    public class RegistryViewModel : ViewModelBase
    {
        private const string BaseSubKeyName = "GitTask";
        private RegistryKey _baseRegistryKey;

        private readonly IProjectPathsService _projectPathsService;

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

        public RegistryViewModel(IProjectPathsService projectPathsService, IRepositoryService repositoryService)
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
                _projectPathsService = projectPathsService;
                if (CurrentProject != null && repositoryService.RepositoryExists(CurrentProject.ProjectPath))
                {
                    _projectPathsService.BaseProjectPath = CurrentProject.ProjectPath;
                }
                _projectPathsService.ProjectPathChanged += ProjectPathsServiceOnProjectPathChanged;
                Messenger.Default.Register<SetCurrentUserMessage>(this, OnSetCurrentUserMessage);
            }
            catch (Exception)
            {
                // Exception. We can't use registry
            }
        }

        private void OnSetCurrentUserMessage(SetCurrentUserMessage message)
        {
            CurrentProject = new RegistryProjectInformation
            {
                ProjectPath = CurrentProject.ProjectPath,
                CurrentUser = message.CurrentUser
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