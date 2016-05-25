using System;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.Messages;
using GitTask.UI.MVVM.Properties;
using GitTask.UI.MVVM.View.ProjectSettings;
using GitTask.UI.MVVM.ViewModel.Common;
using Application = System.Windows.Application;
using ProjectSetupWindow = GitTask.UI.MVVM.View.ProjectSettings.ProjectSetupWindow;

namespace GitTask.UI.MVVM.ViewModel.Main
{
    public class ProjectOpeningViewModel
    {
        private static readonly ResourceManager ResourceManager = new ResourceManager(typeof(Resources));

        private readonly IRepositoryService _repositoryService;
        private readonly IProjectPathsService _projectPathsService;
        private readonly IQueryService<Project> _projectQueryService;
        private readonly RegistryViewModel _registryViewModel;

        private readonly RelayCommand _openSelectFolderDialogCommand;
        public ICommand OpenSelectFolderDialogCommand => _openSelectFolderDialogCommand;


        public ProjectOpeningViewModel(IRepositoryService repositoryService,
                                       IProjectPathsService projectPathsService,
                                       IQueryService<Project> projectQueryService,
                                       RegistryViewModel registryViewModel)
        {
            _repositoryService = repositoryService;
            _projectPathsService = projectPathsService;
            _projectQueryService = projectQueryService;
            _registryViewModel = registryViewModel;

            _projectQueryService.ElementsReloaded += ProjectQueryServiceOnElementsReloaded;
            _openSelectFolderDialogCommand = new RelayCommand(OnOpenSelectFolderDialog);
        }

        private void ProjectQueryServiceOnElementsReloaded()
        {
            if (_projectQueryService.GetAll().Any()) return;
            var projectSetupWindow = new ProjectSetupWindow { Owner = Application.Current.MainWindow };
            projectSetupWindow.ShowDialog();
        }

        private void OnOpenSelectFolderDialog()
        {
            try
            {
                var projectPath = GetProjectPath();
                while (!_repositoryService.RepositoryExists(projectPath))
                {
                    MessageBox.Show(ResourceManager.GetString("RepositoryNotFoundExplanation"),
                                    ResourceManager.GetString("RepositoryNotFound"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    projectPath = GetProjectPath();
                }
                _projectPathsService.BaseProjectPath = projectPath;

                if (_registryViewModel.CurrentProject?.CurrentUser == null)
                {
                    var setCurrentUserWindow = new SetCurrentUserWindow {Owner = Application.Current.MainWindow};
                    setCurrentUserWindow.ShowDialog();
                }
                else
                {
                    Messenger.Default.Send(new SetCurrentUserMessage
                    {
                        CurrentUser = _registryViewModel.CurrentProject.CurrentUser
                    });
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private static string GetProjectPath()
        {
            var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = false,
                Description = ResourceManager.GetString("ChooseRepositoryFolderExplanation"),
            };

            var dialogResult = dialog.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                throw new OperationCanceledException();
            }

            return dialog.SelectedPath;
        }
    }
}