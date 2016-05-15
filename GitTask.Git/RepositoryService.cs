using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using GitTask.Repository.Model;
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

        public IEnumerable<ProjectMember> GetAllCommiters()
        {
            if (_repository == null) return new List<ProjectMember>();

            var nameKeyedDictionary = new Dictionary<string, string>(); // key = name, value = email
            var emailKeyedDictionary = new Dictionary<string, string>(); // key = email, value = name

            // we want unique names, with each project member having the most up-to-date e-mail (from newest commit)
            foreach (var committer in _repository.Commits.Select(commit => commit.Committer).OrderBy(committer => committer.When.DateTime))
            {
                nameKeyedDictionary[committer.Name] = committer.Email;
                emailKeyedDictionary[committer.Email] = committer.Name;
            }

            var keysToRemove = (from nameKey in nameKeyedDictionary.Keys
                                let emailValue = nameKeyedDictionary[nameKey]
                                where emailKeyedDictionary[emailValue] != nameKey
                                select nameKey);

            var keysTomoveHashSet = new HashSet<string>(keysToRemove);

            return nameKeyedDictionary.Where(commiterPair => !keysTomoveHashSet.Contains(commiterPair.Key)).
                                             Select(commiterPair => new ProjectMember(commiterPair.Key, commiterPair.Value));
        }

        public bool RepositoryExists(string projectPath)
        {
            return projectPath != null && Directory.Exists(projectPath) && LibGit2Sharp.Repository.IsValid(projectPath);
        }
    }
}
