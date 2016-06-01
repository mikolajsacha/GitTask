using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.Messages;

namespace GitTask.UI.MVVM.ViewModel.Common
{
    public class ProjectMembersViewModel : ViewModelBase
    {
        private readonly ProjectMembersService _projectMembersService;
        private readonly IRepositoryService _repositoryService;
        private readonly IProjectQueryService _projectQueryService;
        private bool _isLoading;
        public ObservableCollection<ProjectMember> ProjectMembers { get; }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                RaisePropertyChanged();
            }
        }

        public ProjectMembersViewModel(ProjectMembersService projectMembersService,
                                       IRepositoryService repositoryService,
                                       IProjectQueryService projectQueryService)
        {
            _isLoading = false;
            _projectMembersService = projectMembersService;
            _repositoryService = repositoryService;
            _projectQueryService = projectQueryService;

            ProjectMembers = new ObservableCollection<ProjectMember>();

            _repositoryService.RepositoryInitalized += RepositoryServiceOnRepositoryInitalized;
            _projectQueryService.ProjectChanged += ProjectQueryServiceOnProjectChanged;
            ProjectQueryServiceOnProjectChanged(projectQueryService.Project);
            Messenger.Default.Register<AddUserMessage>(this, OnAddUserMessage);
            new Task(async () => await InitializeProjectMembers()).RunSynchronously();
        }

        private async void ProjectQueryServiceOnProjectChanged(Project project)
        {
            await InitializeProjectMembers();
        }

        private async void RepositoryServiceOnRepositoryInitalized()
        {
            await InitializeProjectMembers();
        }

        private async Task InitializeProjectMembers()
        {
            IsLoading = true;
            ProjectMembers.Clear();

            var usersFromProject = _projectQueryService.Project?.ProjectMembersNotInRepository ??
                                   new List<ProjectMember>();
            var usersFromRepo = await _repositoryService.GetAllUniqueCommiters();

            var allUsers = usersFromProject.Concat(usersFromRepo);
            foreach (var user in (await _projectMembersService.GetAllMostRecentUsers(allUsers)).OrderBy(pm => pm.Name))
            {
                ProjectMembers.Add(user);
            }
            IsLoading = false;
        }

        private async void OnAddUserMessage(AddUserMessage message)
        {
            IsLoading = true;
            if (_projectQueryService.Project.ProjectMembersNotInRepository != null &&
                _projectQueryService.Project.ProjectMembersNotInRepository.Contains(message.UserToBeAdded)) return;

            AddUserFromProject(message.UserToBeAdded);

            _projectQueryService.AddUser(message.UserToBeAdded);
            IsLoading = false;
            await _projectQueryService.SaveChanges();
        }

        private void AddUserFromProject(ProjectMember user)
        {
            foreach(var member in ProjectMembers.Where(member => member.Name == user.Name || member.Email == user.Email).ToList())
            {
                ProjectMembers.Remove(member);
            }
            ProjectMembers.Add(user);
            ProjectMembers.Move(ProjectMembers.Count - 1, 0);
        }
    }
}