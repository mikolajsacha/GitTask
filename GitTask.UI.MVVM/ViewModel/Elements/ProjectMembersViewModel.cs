using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GitTask.Repository.Model;
using GitTask.Repository.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.Elements
{
    public class ProjectMembersViewModel : ViewModelBase
    {
        private readonly IRepositoryService _repositoryService;
        public ObservableCollection<ProjectMember> ProjectMembers { get; }

        public ProjectMembersViewModel(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
            ProjectMembers = new ObservableCollection<ProjectMember>();

            _repositoryService.RepositoryInitalized += RepositoryServiceOnRepositoryInitalized;
            RepositoryServiceOnRepositoryInitalized();
        }

        private void RepositoryServiceOnRepositoryInitalized()
        {
            ProjectMembers.Clear();
            foreach (var commiter in _repositoryService.GetAllCommiters().OrderBy(commiter => commiter.Name))
            {
                ProjectMembers.Add(commiter);
            }
        }
    }
}