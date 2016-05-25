using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.Messages;
using GitTask.UI.MVVM.ViewModel.Common;

namespace GitTask.UI.MVVM.ViewModel.Footer
{
    public class FooterViewModel : ViewModelBase
    {
        private readonly IQueryService<Project> _projectQueryService;

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

        private ProjectMember _currentUser;
        public ProjectMember CurrentUser
        {
            get { return _currentUser; }
            private set
            {
                _currentUser = value;
                RaisePropertyChanged();
            }
        }

        public FooterViewModel(IQueryService<Project> projectQueryService, RegistryViewModel registryViewModel)
        {
            _projectQueryService = projectQueryService;
            var projects = _projectQueryService.GetAll().ToList();
            if (projects.Any())
            {
                _projectName = projects.First().Title;
            }

            projectQueryService.ElementAdded += ProjectQueryServiceOnElementAdded;
            projectQueryService.ElementsReloaded += ProjectQueryServiceOnElementsReloaded;

            CurrentUser = registryViewModel.CurrentProject.CurrentUser;
            Messenger.Default.Register<SetCurrentUserMessage>(this, OnSetCurrentUserMessage);
        }

        private void ProjectQueryServiceOnElementsReloaded()
        {
            var projects = _projectQueryService.GetAll().ToList();
            if (projects.Any())
            {
                ProjectName = projects.First().Title;
            }
        }

        private void OnSetCurrentUserMessage(SetCurrentUserMessage currentUserMessage)
        {
            CurrentUser = currentUserMessage.CurrentUser;
        }

        private void ProjectQueryServiceOnElementAdded(Project project)
        {
            ProjectName = project.Title;
        }
    }
}