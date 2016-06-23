using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;

namespace GitTask.Storage
{
    public class ProjectQueryService : IProjectQueryService
    {
        private readonly IStorageService<Project> _storageService;

        public Project Project { get; private set; }

        public event Action<Project> ProjectChanged;
        public event Action<string> ProjectTitleChanged;
        public event Action<ProjectMember> UserAdded;

        public ProjectQueryService(IStorageService<Project> storageService,
                                   IMergingService mergingService)
        {
            _storageService = storageService;

            mergingService.MergingCompleted += InitializeDataFromStorage;
            if (mergingService.IsMergingCompleted)
            {
                InitializeDataFromStorage();
            }
        }

        private async void InitializeDataFromStorage()
        {
            var data = (await _storageService.GetAll()).ToList();
            Project = data.Any() ? data.First() : null;
            ProjectChanged?.Invoke(Project);
            if (Project != null)
            {
                ProjectTitleChanged?.Invoke(Project.Title);
            }
        }

        public void SetTitle(string title)
        {
            if (Project == null)
            {
                Project = new Project();
            }
            Project.Title = title;
            ProjectTitleChanged?.Invoke(title);
        }

        public void AddUser(ProjectMember user)
        {
            if (Project == null) return;
            if (Project.ProjectMembersNotInRepository == null)
            {
                Project.ProjectMembersNotInRepository = new List<ProjectMember>();
            }
            Project.ProjectMembersNotInRepository.Add(user);
            UserAdded?.Invoke(user);
        }

        public async Task SaveChanges()
        {
            await _storageService.Save(Project);
        }
    }
}