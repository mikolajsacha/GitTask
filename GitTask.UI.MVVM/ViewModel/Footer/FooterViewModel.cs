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
            _projectName = projectQueryService.GetAll().First().Title;
            projectQueryService.ElementAdded += ProjectQueryServiceOnElementAdded;

            ProjectMembers = new ObservableCollection<string>(repositoryService.GetAllCommitersNames());
            CurrentUser = ProjectMembers.First(); //TODO: wybrac/stworzyc uzytkownika
        }

        private void ProjectQueryServiceOnElementAdded(Project project)
        {
            ProjectName = project.Title;
        }
    }
}