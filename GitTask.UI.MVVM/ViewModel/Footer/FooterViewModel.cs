using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GitTask.Repository.Services.Interface;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.Footer
{
    public class FooterViewModel : ViewModelBase
    {
        private readonly IRepositoryService _repositoryService;
        public ObservableCollection<string> ProjectMembers { get; }

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

        public string CurrentUser { get; }

        public FooterViewModel(IQueryService<Project> projectQueryService, IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;

            var projects = projectQueryService.GetAll().ToList();
            if (projects.Any())
            {
                _projectName = projects.First().Title;
            }

            projectQueryService.ElementAdded += ProjectQueryServiceOnElementAdded;
            _repositoryService.RepositoryInitalized += RepositoryServiceOnRepositoryInitalized;

            ProjectMembers = new ObservableCollection<string>();
            RepositoryServiceOnRepositoryInitalized();

            if (ProjectMembers.Any())
            {
                CurrentUser = ProjectMembers.First(); //TODO: wybrac/stworzyc uzytkownika
            }
        }

        private void RepositoryServiceOnRepositoryInitalized()
        {
            ProjectMembers.Clear();
            foreach (var commiter in _repositoryService.GetAllCommitersNames())
            {
                ProjectMembers.Add(commiter);
            }
        }

        private void ProjectQueryServiceOnElementAdded(Project project)
        {
            ProjectName = project.Title;
        }
    }
}