using System;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.Properties;
using GitTask.UI.MVVM.View.ProjectSettings;
using Application = System.Windows.Application;
using ProjectSetupWindow = GitTask.UI.MVVM.View.ProjectSettings.ProjectSetupWindow;

namespace GitTask.UI.MVVM.ViewModel.Main
{
    public class ProjectOpeningViewModel
    {
        private static readonly ResourceManager ResourceManager = new ResourceManager(typeof(Resources));

        private readonly IRepositoryService _repositoryService;
        private readonly IProjectPathsService _projectPathsService;

        private readonly RelayCommand _openSelectFolderDialogCommand;
        public ICommand OpenSelectFolderDialogCommand => _openSelectFolderDialogCommand;


        public ProjectOpeningViewModel(IRepositoryService repositoryService,
                                       IProjectPathsService projectPathsService,
                                       IProjectQueryService projectQueryService)
        {
            _repositoryService = repositoryService;
            _projectPathsService = projectPathsService;

            projectQueryService.ProjectChanged += ProjectQueryServiceOnChanged;
            _openSelectFolderDialogCommand = new RelayCommand(OnOpenSelectFolderDialog);
        }

        private static void ProjectQueryServiceOnChanged(Project newProject)
        {
            if (newProject != null) return;
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

                var setCurrentUserWindow = new SetCurrentUserWindow { Owner = Application.Current.MainWindow };
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