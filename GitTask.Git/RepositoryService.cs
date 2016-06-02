using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GitTask.Domain.Attributes;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Repository;
using GitTask.Domain.Services.Interface;
using GitTask.Storage.Interface;
using LibGit2Sharp;

namespace GitTask.Git
{
    public class RepositoryService : IRepositoryService
    {
        public event Action RepositoryInitalized;

        private readonly IProjectPathsReadonlyService _projectPathsService;
        private readonly IFileService _fileService;
        private Repository _repository;

        public RepositoryService(IProjectPathsReadonlyService projectPathsService, IFileService fileService)
        {
            _projectPathsService = projectPathsService;
            _fileService = fileService;
            _projectPathsService.ProjectPathChanged += OnProjectPathChanged;
            OnProjectPathChanged();
        }

        private void OnProjectPathChanged()
        {
            if (!RepositoryExists(_projectPathsService.BaseProjectPath)) return;

            _repository = new Repository(_projectPathsService.BaseProjectPath);
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

        public async Task<DateTime> GetCreationDate<TModel>(TModel modelObject)
        {
            return await Task.Run(() =>
            {
                if (_repository == null) return DateTime.Now;

                var objectPath = GetObjectPath(modelObject);
                foreach (var commit in _repository.Commits.QueryBy(new CommitFilter { FirstParentOnly = true }))
                {
                    try
                    {
                        if (!commit.Parents.Any())
                        {
                            return commit.Tree.Any(treeEntry => treeEntry.Path == objectPath) ? commit.Committer.When.DateTime : DateTime.Now;
                        }

                        var parentCommit = commit.Parents.First();
                        var treeChanges = _repository.Diff.Compare<TreeChanges>(parentCommit.Tree, commit.Tree, new CompareOptions());
                        if (treeChanges.Where(treeEntryChanges => treeEntryChanges.Path == objectPath)
                                         .Any(treeEntryChanges => treeEntryChanges.Exists && !treeEntryChanges.OldExists))
                        {
                            return commit.Committer.When.DateTime;
                        }
                    }
                    catch (Exception)
                    {
                        // exception while lazy loading commit parents. Skip commit
                    }
                }

                return DateTime.Now;
            });
        }

        public bool RepositoryExists(string projectPath)
        {
            return projectPath != null && Directory.Exists(projectPath) && Repository.IsValid(projectPath);
        }

        private string GetObjectPath<TModel>(TModel modelObject)
        {
            return $"{_projectPathsService.RelativeStoragePath}\\" +
                   $"{typeof(TModel).Name}\\" +
                   $"{KeyAttribute.GetKeyProperty(typeof(TModel)).GetValue(modelObject)}" +
                   $"{_fileService.FilesExtension}";
        }
    }
}
