using System;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GitTask.Repository.Services.Interface;
using GitTask.UI.MVVM.Properties;
using GitTask.UI.MVVM.View.Main;

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
                                       IProjectPathsService projectPathsService)
        {
            _repositoryService = repositoryService;
            _projectPathsService = projectPathsService;

            _openSelectFolderDialogCommand = new RelayCommand(OnOpenSelectFolderDialog);
        }

        private void OnOpenSelectFolderDialog()
        {
            OpenProject();
        }

        private void OpenProject()
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
            }
            catch (OperationCanceledException)
            {
                return;
            }
            var projectSetupWindow = new ProjectSetupWindow();
            projectSetupWindow.ShowDialog();
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