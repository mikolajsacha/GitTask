using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GitTask.Domain.Attributes;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Repository.EntityHistory;
using GitTask.Domain.Model.Repository.ProjectHistory;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;
using GitTask.Storage.Interface;
using LibGit2Sharp;
using Task = System.Threading.Tasks.Task;

namespace GitTask.Git
{
    public class RepositoryService : IRepositoryService
    {
        public event Action RepositoryInitalized;

        private readonly IProjectPathsReadonlyService _projectPathsService;
        private readonly IFileService _fileService;
        private readonly HistoryResolvingService _historyResolvingService;
        private Repository _repository;

        public RepositoryService(IProjectPathsReadonlyService projectPathsService, IFileService fileService)
        {
            _projectPathsService = projectPathsService;
            _fileService = fileService;
            _projectPathsService.ProjectPathChanged += OnProjectPathChanged;
            _historyResolvingService = new HistoryResolvingService(fileService);
            OnProjectPathChanged();
        }

        public bool RepositoryExists(string projectPath)
        {
            return projectPath != null && Directory.Exists(projectPath) && Repository.IsValid(projectPath);
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

        public async Task<ProjectHistory> GetProjectHistory()
        {
            return await Task.Run(() =>
            {
                if (_repository == null) return null;

                var projectHistory = new ProjectHistory { Changes = new List<ProjectCommitChange>() };

                var baseProjectPath = GetBaseEntityPath("Project");
                var baseTaskPath = GetBaseEntityPath("Task");
                var projectInRepo = false;

                foreach (var commit in _repository.Commits.QueryBy(new CommitFilter { FirstParentOnly = true }))
                {
                    try
                    {
                        if (!commit.Parents.Any())
                        {
                            return !projectInRepo ? null : projectHistory;
                        }

                        var parentCommit = commit.Parents.First();
                        var treeChanges = _repository.Diff.Compare<TreeChanges>(parentCommit.Tree, commit.Tree).ToList();

                        var commitChange = new ProjectCommitChange
                        {
                            Author = new ProjectMember(commit.Committer.Name, commit.Committer.Email),
                            Date = commit.Committer.When.DateTime,
                            AddedTasks = HistoryResolvingService.GetAddedTasks(treeChanges, baseTaskPath).ToList(),
                            RemovedTasks = HistoryResolvingService.GetRemovedTasks(treeChanges, baseTaskPath).ToList(),
                            ProjectMembersChange =
                                _historyResolvingService.GetProjectMembersChange(parentCommit, commit, treeChanges, baseProjectPath),
                            TaskStatesChange = _historyResolvingService.GetTaskStatesChange(parentCommit, commit, treeChanges, _projectPathsService.RelativeStoragePath, typeof(TaskState).Name),
                        };
                        if (commitChange.AddedTasks.Any() || commitChange.RemovedTasks.Any() ||
                            commitChange.ProjectMembersChange != null || commitChange.TaskStatesChange != null)
                        {
                            projectInRepo = true;
                            projectHistory.Changes.Add(commitChange);
                            projectHistory.CreationDate = commit.Committer.When.DateTime;
                        }
                    }
                    catch (Exception)
                    {
                        // exception while traversing repo or parsing files. Skip commit
                    }
                }
                projectHistory.CreationDate = DateTime.MinValue;
                return projectHistory;
            });
        }

        public async Task<EntityHistory> GetEntityHistory<TModel>(TModel modelObject)
        {
            return await Task.Run(() =>
            {
                if (_repository == null) return null;

                var entityHistory = new EntityHistory { Changes = new List<EntityCommitChange>() };
                var objectPath = GetObjectPath(modelObject);
                var entityInRepo = false;

                foreach (var commit in _repository.Commits.QueryBy(new CommitFilter { FirstParentOnly = true }))
                {
                    try
                    {
                        if (!commit.Parents.Any())
                        {
                            return entityInRepo ? entityHistory : null;
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

                        var propertyChanges = _historyResolvingService.ResolveChangesInBlob<TModel>(parentBlob, childBlob).ToList();
                        if (!propertyChanges.Any()) continue;

                        var entityCommitChange = new EntityCommitChange
                        {
                            PropertyChanges = propertyChanges,
                            Date = commit.Committer.When.DateTime,
                            Author = new ProjectMember(commit.Committer.Name, commit.Committer.Email)
                        };
                        entityHistory.Changes.Add(entityCommitChange);
                        entityInRepo = true;
                        entityHistory.CreationDate = commit.Committer.When.DateTime;
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

        private void OnProjectPathChanged()
        {
            if (!RepositoryExists(_projectPathsService.BaseProjectPath)) return;

            _repository = new Repository(_projectPathsService.BaseProjectPath);
            RepositoryInitalized?.Invoke();
        }

        private string GetObjectPath<TModel>(TModel modelObject)
        {
            return GetBaseEntityPath(typeof(TModel).Name) +
                   $"{KeyAttribute.GetKeyProperty(typeof(TModel)).GetValue(modelObject)}" +
                   $"{_fileService.FilesExtension}";
        }

        private string GetBaseEntityPath(string typeName)
        {
            return $"{_projectPathsService.RelativeStoragePath}\\" +
                   $"{typeName}\\";
        }
    }
}
