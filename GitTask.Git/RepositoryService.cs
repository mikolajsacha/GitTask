using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;

namespace GitTask.Git
{
    public class RepositoryService : IRepositoryService
    {
        public event Action RepositoryInitalized;

        private readonly IProjectPathsReadonlyService _projectPathsService;
        private LibGit2Sharp.Repository _repository;

        public RepositoryService(IProjectPathsReadonlyService projectPathsService)
        {
            _projectPathsService = projectPathsService;
            _projectPathsService.ProjectPathChanged += OnProjectPathChanged;
            OnProjectPathChanged();
        }

        private void OnProjectPathChanged()
        {
            if (!RepositoryExists(_projectPathsService.BaseProjectPath)) return;

            _repository = new LibGit2Sharp.Repository(_projectPathsService.BaseProjectPath);
            RepositoryInitalized?.Invoke();
        }

        public async Task<IEnumerable<ProjectMember>> GetAllUniqueCommiters()
        {
            return await Task.Run(() =>
            {
                if (_repository == null) return new List<ProjectMember>();

                return _repository.Commits.Select(commit => commit.Committer)
                       .OrderBy(committer => committer.When.DateTime)
                       .Select(commiter => new ProjectMember(commiter.Name, commiter.Email))
                       .Distinct();
            });
        }

        public bool RepositoryExists(string projectPath)
        {
            return projectPath != null && Directory.Exists(projectPath) && LibGit2Sharp.Repository.IsValid(projectPath);
        }
    }
}
