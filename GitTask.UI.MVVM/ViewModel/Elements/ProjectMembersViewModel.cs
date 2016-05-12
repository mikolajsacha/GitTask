using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GitTask.Repository.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.Elements
{
    public class ProjectMembersViewModel : ViewModelBase
    {
        private readonly IRepositoryService _repositoryService;
        public ObservableCollection<string> ProjectMembers { get; }

        public ProjectMembersViewModel(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
            _repositoryService.RepositoryInitalized += RepositoryServiceOnRepositoryInitalized;

            ProjectMembers = new ObservableCollection<string>();
            RepositoryServiceOnRepositoryInitalized();
        }

        private void RepositoryServiceOnRepositoryInitalized()
        {
            ProjectMembers.Clear();
            foreach (var commiter in _repositoryService.GetAllCommitersNames())
            {
                ProjectMembers.Add(commiter);
            }
        }
    }
}