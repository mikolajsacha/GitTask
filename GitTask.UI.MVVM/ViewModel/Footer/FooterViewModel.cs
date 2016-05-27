using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.ViewModel.Common;

namespace GitTask.UI.MVVM.ViewModel.Footer
{
    public class FooterViewModel : ViewModelBase
    {
        private readonly IQueryService<Project> _projectQueryService;
        private readonly CurrentUserViewModel _currentUserViewModel;

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

        public FooterViewModel(IQueryService<Project> projectQueryService,
                               RegistryViewModel registryViewModel,
                               CurrentUserViewModel currentUserViewModel)
        {
            _projectQueryService = projectQueryService;
            _currentUserViewModel = currentUserViewModel;

            var projects = _projectQueryService.GetAll().ToList();
            if (projects.Any())
            {
                _projectName = projects.First().Title;
            }

            projectQueryService.ElementAdded += ProjectQueryServiceOnElementAdded;
            projectQueryService.ElementsReloaded += ProjectQueryServiceOnElementsReloaded;
        }

        private void ProjectQueryServiceOnElementsReloaded()
        {
            var projects = _projectQueryService.GetAll().ToList();
            if (projects.Any())
            {
                ProjectName = projects.First().Title;
            }
        }

        private void ProjectQueryServiceOnElementAdded(Project project)
        {
            ProjectName = project.Title;
        }
    }
}