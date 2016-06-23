using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.Messages;

namespace GitTask.UI.MVVM.ViewModel.Common
{
    public class ProjectMembersViewModel : ViewModelBase
    {
        private readonly IRepositoryService _repositoryService;
        private readonly IProjectQueryService _projectQueryService;
        public ObservableCollection<ProjectMember> ProjectMembers { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                RaisePropertyChanged();
            }
        }

        private bool _repositoryInitialized;
        private bool _projectChanged;

        public ProjectMembersViewModel(IRepositoryService repositoryService,
                                       IProjectQueryService projectQueryService)
        {
            _isLoading = false;
            _repositoryService = repositoryService;
            _projectQueryService = projectQueryService;

            ProjectMembers = new ObservableCollection<ProjectMember>();

            _repositoryService.RepositoryInitalized += RepositoryServiceOnRepositoryInitalized;
            _projectQueryService.ProjectChanged += ProjectQueryServiceOnProjectChanged;
            Messenger.Default.Register<AddUserMessage>(this, OnAddUserMessage);
            new Task(async () =>
            {
                if (projectQueryService.Project != null)
                {
                    await UpdateProjectMembers();
                }
                if (repositoryService.IsRepositoryInitialized)
                {
                    await UpdateRepositoryMembers();
                }
            }).RunSynchronously();
        }

        private async void ProjectQueryServiceOnProjectChanged(Project project)
        {
            await UpdateProjectMembers();
        }
        private async void RepositoryServiceOnRepositoryInitalized()
        {
            await UpdateRepositoryMembers();
        }

        private async Task UpdateProjectMembers()
        {
            _projectChanged = true;
            await TryInitializeProject();
        }

        private async Task UpdateRepositoryMembers()
        {
            _repositoryInitialized = true;
            await TryInitializeProject();
        }

        private async Task TryInitializeProject()
        {
            if (_repositoryInitialized && _projectChanged)
            {
                _repositoryInitialized = false;
                _projectChanged = false;
                await InitializeProjectMembers();
            }
        }

        private async Task InitializeProjectMembers()
        {
            IsLoading = true;
            ProjectMembers.Clear();

            var usersFromProject = _projectQueryService.Project?.ProjectMembersNotInRepository ??
                                   new List<ProjectMember>();
            var usersFromRepo = await _repositoryService.GetAllUniqueCommiters();

            foreach (var user in usersFromProject.Concat(usersFromRepo).Distinct().OrderBy(pm => pm.Name))
            {
                ProjectMembers.Add(user);
            }
            IsLoading = false;
        }

        private async void OnAddUserMessage(AddUserMessage message)
        {
            IsLoading = true;
            if (_projectQueryService.Project.ProjectMembersNotInRepository != null &&
                _projectQueryService.Project.ProjectMembersNotInRepository.Contains(message.UserToBeAdded))
            {
                IsLoading = false;
                return;
            }

            AddUserFromProject(message.UserToBeAdded);

            _projectQueryService.AddUser(message.UserToBeAdded);
            IsLoading = false;
            await _projectQueryService.SaveChanges();
        }

        private void AddUserFromProject(ProjectMember user)
        {
            foreach (var member in ProjectMembers.Where(member => member.Name == user.Name || member.Email == user.Email).ToList())
            {
                ProjectMembers.Remove(member);
            }
            ProjectMembers.Add(user);
            ProjectMembers.Move(ProjectMembers.Count - 1, 0);
        }
    }
}