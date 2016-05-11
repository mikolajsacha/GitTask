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
        private string _projectName;
        public ObservableCollection<string> ProjectMembers { get; }

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
            var projects = projectQueryService.GetAll().ToList();
            if (projects.Any())
            {
                _projectName = projects.First().Title;
            }
            projectQueryService.ElementAdded += ProjectQueryServiceOnElementAdded;

            ProjectMembers = new ObservableCollection<string>(repositoryService.GetAllCommitersNames());
            if (ProjectMembers.Any())
            {
                CurrentUser = ProjectMembers.First(); //TODO: wybrac/stworzyc uzytkownika
            }
        }

        private void ProjectQueryServiceOnElementAdded(Project project)
        {
            ProjectName = project.Title; //TODO: sprawdzic, czemu sie nie wywoluje == naprawic footer
        }
    }
}