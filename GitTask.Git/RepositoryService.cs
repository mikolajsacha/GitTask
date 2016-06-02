using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GitTask.Domain.Attributes;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Repository;
using GitTask.Domain.Services.Interface;
using GitTask.Json;
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

        public async Task<EntityHistory> GetHistory<TModel>(TModel modelObject)
        {
            return await Task.Run(() =>
            {
                if (_repository == null) return null;

                var entityHistory = new EntityHistory { Changes = new List<EntityCommitChange>() };

                var objectPath = GetObjectPath(modelObject);

                var commits = _repository.Commits.QueryBy(new CommitFilter { FirstParentOnly = true });
                if (!commits.Any()) return null;

                foreach (var commit in _repository.Commits.QueryBy(new CommitFilter { FirstParentOnly = true }))
                {
                    try
                    {
                        if (!commit.Parents.Any())
                        {
                            entityHistory.CreationDate = commit.Committer.When.DateTime;
                            entityHistory.Author = new ProjectMember(commit.Committer.Name, commit.Committer.Email);
                            return entityHistory;
                        }

                        var parentCommit = commit.Parents.First();
                        var treeChanges = _repository.Diff.Compare<TreeChanges>(parentCommit.Tree, commit.Tree, new CompareOptions());
                        var entityTreeChanges = treeChanges.Where(treeEntry => treeEntry.Path == objectPath).ToList();
                        if (!entityTreeChanges.Any()) continue;

                        var treeEntryChanges = entityTreeChanges.First();

                        if (treeEntryChanges.Exists && !treeEntryChanges.OldExists)
                        {
                            entityHistory.CreationDate = commit.Committer.When.DateTime;
                            entityHistory.Author = new ProjectMember(commit.Committer.Name, commit.Committer.Email);
                            return entityHistory;
                        }
                        var parentTreeEntry = parentCommit[objectPath];
                        var childTreeEntry = commit[objectPath];

                        if (parentTreeEntry.TargetType != TreeEntryTargetType.Blob ||
                            childTreeEntry.TargetType != TreeEntryTargetType.Blob) continue;

                        var parentBlob = (Blob)parentTreeEntry.Target;
                        var childBlob = (Blob)childTreeEntry.Target;

                        var entityCommitChange = new EntityCommitChange
                        {
                            PropertyChanges = ResolveChanges<TModel>(parentBlob, childBlob).ToList(),
                            Date = commit.Committer.When.DateTime
                        };
                        if (entityCommitChange.PropertyChanges != null && entityCommitChange.PropertyChanges.Any())
                        {
                            entityHistory.Changes.Add(entityCommitChange);
                        }
                    }
                    catch (Exception)
                    {
                        // exception while traversing repo or parsing files. Skip commit
                    }
                }
                entityHistory.CreationDate = DateTime.MinValue;
                return entityHistory;
            });
        }

        private IEnumerable<EntityPropertyChange> ResolveChanges<TModel>(Blob parentBlob, Blob childBlob)
        {
            var parentContentStream = parentBlob.GetContentStream();
            var childContentStream = childBlob.GetContentStream();

            TModel parentObject;
            TModel childObject;

            var propertyChanges = new List<EntityPropertyChange>();

            try
            {
                using (var streamReader = new StreamReader(parentContentStream, BufferWorker.Encoding))
                {
                    var parentContent = streamReader.ReadToEnd();
                    parentObject = _fileService.ParseString<TModel>(parentContent);
                }

                using (var streamReader = new StreamReader(childContentStream, BufferWorker.Encoding))
                {
                    var childContent = streamReader.ReadToEnd();
                    childObject = _fileService.ParseString<TModel>(childContent);
                }
            }
            catch (Exception) // exception while parsing
            {
                return null;
            }

            var properties = typeof (TModel).GetProperties();

            //TODO: Zmienić logikę dla kolekcji (tak, aby kolekcje o takich samych elementach nie byly wliczane do zmian)
            foreach (var property in typeof(TModel).GetProperties())
            {
                var parentPropertyValue = property.GetValue(parentObject);
                var childPropertyValue = property.GetValue(childObject);
                if (!parentPropertyValue.Equals(childPropertyValue))
                {
                    propertyChanges.Add(new EntityPropertyChange { OldValue = parentPropertyValue, PropertyName = property.Name });
                }
            }

            return propertyChanges;
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
