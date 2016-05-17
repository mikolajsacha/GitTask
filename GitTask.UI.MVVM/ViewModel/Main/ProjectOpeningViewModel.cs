using System;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.Properties;
using GitTask.UI.MVVM.View.ProjectSettings;
using ProjectSetupWindow = GitTask.UI.MVVM.View.ProjectSettings.ProjectSetupWindow;

namespace GitTask.UI.MVVM.ViewModel.Main
{
    public class ProjectOpeningViewModel
    {
        private static readonly ResourceManager ResourceManager = new ResourceManager(typeof(Resources));

        private readonly IRepositoryService _repositoryService;
        private readonly IProjectPathsService _projectPathsService;
        private readonly IQueryService<Project> _projectQueryService;

        private readonly RelayCommand _openSelectFolderDialogCommand;
        public ICommand OpenSelectFolderDialogCommand => _openSelectFolderDialogCommand;


        public ProjectOpeningViewModel(IRepositoryService repositoryService,
                                       IProjectPathsService projectPathsService,
                                       IQueryService<Project> projectQueryService)
        {
            _repositoryService = repositoryService;
            _projectPathsService = projectPathsService;
            _projectQueryService = projectQueryService;

            _projectQueryService.ElementsReloaded += ProjectQueryServiceOnElementsReloaded;
            _openSelectFolderDialogCommand = new RelayCommand(OnOpenSelectFolderDialog);
        }

        private void ProjectQueryServiceOnElementsReloaded()
        {
            if (!_projectQueryService.GetAll().Any())
            {
                var projectSetupWindow = new ProjectSetupWindow();
                projectSetupWindow.ShowDialog();
            }
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

                var setCurrentUserWindow = new SetCurrentUserWindow();
                setCurrentUserWindow.ShowDialog();
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
                Description = ResourceManager.GetString("ChooseRepositoryFolderExplanation")
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