using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.Common
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

        private async void RepositoryServiceOnRepositoryInitalized()
        {
            ProjectMembers.Clear();
            var allCommiters = await _repositoryService.GetAllCommiters();
            foreach (var commiter in allCommiters.OrderBy(commiter => commiter.Name))
            {
                ProjectMembers.Add(commiter);
            }
        }
    }
}