using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.Locator;
using GitTask.UI.MVVM.Properties;

namespace GitTask.UI.MVVM.ViewModel.Footer
{
    public class FooterViewModel : ViewModelBase
    {
        private string _projectName;
        public string ProjectName
        {
            get { return _projectName; }
            private set
            {
                _projectName = value;
                RaisePropertyChanged();
            }
        }

        private readonly RelayCommand _showCreditsCommand;
        public ICommand ShowCreditsCommand => _showCreditsCommand;

        public FooterViewModel(IProjectQueryService projectQueryService)
        {
            _showCreditsCommand = new RelayCommand(OnShowCreditsCommand);
            if (projectQueryService.Project != null)
            {
                _projectName = projectQueryService.Project.Title;
            }

            projectQueryService.ProjectTitleChanged += ProjectQueryServiceOnTitleChanged;
        }

        private void OnShowCreditsCommand()
        {
            MessageBox.Show(IocLocator.ResourceManager.GetString("CreditsProgramAuthorInfo") + "\n" + IocLocator.ResourceManager.GetString("CreditsIconAuthorInfo"),
                IocLocator.ResourceManager.GetString("About"));
        }

        private void ProjectQueryServiceOnTitleChanged(string newTitle)
        {
            if (newTitle != null)
            {
                ProjectName = newTitle;
            }
        }
    }
}