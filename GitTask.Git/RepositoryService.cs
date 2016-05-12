using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GitTask.Repository.Services.Interface;

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

        public IEnumerable<string> GetAllCommitersNames()
        {
            return _repository?.Commits.Select(x => x.Author.Name).Distinct() ?? new List<string>();
        }

        public bool RepositoryExists(string projectPath)
        {
            return projectPath != null && Directory.Exists(projectPath) && LibGit2Sharp.Repository.IsValid(projectPath);
        }
    }
}
