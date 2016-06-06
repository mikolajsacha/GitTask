using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Repository.EntityHistory;
using GitTask.Domain.Model.Repository.Merging;
using GitTask.Domain.Model.Task;
using GitTask.Json;
using GitTask.Storage.Interface;
using LibGit2Sharp;

namespace GitTask.Git
{
    public class HistoryResolvingService
    {
        private readonly IFileService _fileService;

        public HistoryResolvingService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public static IEnumerable<string> GetRemovedTasks(IList<TreeEntryChanges> treeChanges, string baseTaskPath)
        {
            var removedTasks = new List<string>();
            foreach (var taskRemovedChange in
                               treeChanges.Where(treeEntryChange => treeEntryChange.Path.StartsWith(baseTaskPath)
                               && !treeEntryChange.Exists && treeEntryChange.OldExists))
            {
                var taskName = Path.GetFileNameWithoutExtension(taskRemovedChange.Path);
                removedTasks.Add(taskName);
            }
            return removedTasks;
        }

        public static IEnumerable<string> GetAddedTasks(IList<TreeEntryChanges> treeChanges, string baseTaskPath)
        {
            var addedTasks = new List<string>();
            foreach (var taskAddedChange in
                treeChanges.Where(treeEntryChange => treeEntryChange.Path.StartsWith(baseTaskPath)
                                                     && treeEntryChange.Exists && !treeEntryChange.OldExists))
            {
                var taskName = Path.GetFileNameWithoutExtension(taskAddedChange.Path);
                addedTasks.Add(taskName);
            }
            return addedTasks;
        }

        public EntityPropertyChange GetProjectMembersChange(Commit parentCommit, Commit commit,
                                                             IEnumerable<TreeEntryChanges> treeChanges, string baseProjectPath)
        {
            foreach (var projectChange in
                              treeChanges.Where(treeEntryChange => treeEntryChange.Path.StartsWith(baseProjectPath)))
            {
                if (!projectChange.Exists || !projectChange.OldExists) continue;

                var parentTreeEntry = parentCommit[projectChange.Path];
                var childTreeEntry = commit[projectChange.Path];

                if (parentTreeEntry.TargetType != TreeEntryTargetType.Blob ||
                    childTreeEntry.TargetType != TreeEntryTargetType.Blob) continue;

                var parentBlob = (Blob)parentTreeEntry.Target;
                var childBlob = (Blob)childTreeEntry.Target;

                var changes = ResolveChangesInBlob<Project>(parentBlob, childBlob);
                foreach (
                    var projectMemberChange in
                        changes.Where(change => change.PropertyName == "ProjectMembersNotInRepository").Take(1))
                {
                    return projectMemberChange;
                }
            }
            return null;
        }

        public EntityPropertyChange GetTaskStatesChange(Commit parentCommit, Commit commit, IEnumerable<TreeEntryChanges> treeChanges, string baseRepoPath, string relativeTaskStatesPath)
        {
            if (!treeChanges.Any(x => x.Path.Contains(relativeTaskStatesPath)))
            {
                return null;
            }

            var oldTaskStates = GetTaskStatesFromCommit(parentCommit, baseRepoPath, relativeTaskStatesPath) ??
                                new List<TaskState>();
            var newTaskStates = GetTaskStatesFromCommit(commit, baseRepoPath, relativeTaskStatesPath) ??
                                new List<TaskState>();

            if (!newTaskStates.Any() && !oldTaskStates.Any()) return null;
            return new EntityPropertyChange
            {
                PropertyName = "TaskStates",
                NewValue = newTaskStates,
                OldValue = oldTaskStates
            };
        }

        public static bool AreEnumerablePropertiesEqual(object parentPropertyValue, object childPropertyValue)
        {
            var parentEnumerator = ((IEnumerable)parentPropertyValue).GetEnumerator();
            var childEnumerator = ((IEnumerable)childPropertyValue).GetEnumerator();
            while (parentEnumerator.MoveNext())
            {
                if (!childEnumerator.MoveNext()) return false; // child is shorter
                if (!parentEnumerator.Current.Equals(childEnumerator.Current)) return false; // different element values
            }
            return !childEnumerator.MoveNext(); // check if child is longer;
        }

        public IEnumerable<EntityPropertyChange> ResolveChangesInBlob<TModel>(Blob parentBlob, Blob childBlob)
        {
            try
            {
                var parentObject = GetEntityObjectFromStream<TModel>(parentBlob.GetContentStream());
                var childObject = GetEntityObjectFromStream<TModel>(childBlob.GetContentStream());
                return ResolveChangesInObjects(parentObject, childObject);
            }
            catch (Exception) // exception while parsing
            {
                return new List<EntityPropertyChange>();
            }
        }

        private IEnumerable<TaskState> GetTaskStatesFromCommit(Commit commit, string baseRepoPath, string relativeTaskStatesPath)
        {
            var taskStates = new List<TaskState>();

            var repoFolderEntries = commit.Tree.Where(treeEntry => treeEntry.Path.Contains(baseRepoPath)).ToList();
            if (!repoFolderEntries.Any() || repoFolderEntries.First().TargetType != TreeEntryTargetType.Tree) return null;

            var taskStateFolderEntries = ((Tree)repoFolderEntries.First().Target).Where(treeEntry => treeEntry.Path.Contains(relativeTaskStatesPath)).ToList();
            if (!taskStateFolderEntries.Any() || taskStateFolderEntries.First().TargetType != TreeEntryTargetType.Tree) return null;

            foreach (var taskStateEntry in (Tree)taskStateFolderEntries.First().Target)
            {
                var taskStateTreeEntry = commit[taskStateEntry.Path];
                if (taskStateTreeEntry.TargetType != TreeEntryTargetType.Blob) continue;
                var blob = (Blob)taskStateTreeEntry.Target;
                var contentStream = blob.GetContentStream();

                TaskState taskStateObject;
                try
                {
                    taskStateObject = GetEntityObjectFromStream<TaskState>(contentStream);
                }
                catch (Exception) // exception while parsing
                {
                    continue;
                }
                taskStates.Add(taskStateObject);
            }
            return taskStates;
        }

        private static IEnumerable<EntityPropertyChange> ResolveChangesInObjects<TModel>(TModel parentObject, TModel childObject)
        {
            var propertyChanges = new List<EntityPropertyChange>();
            foreach (var property in typeof(TModel).GetProperties())
            {
                var parentPropertyValue = property.GetValue(parentObject);
                var childPropertyValue = property.GetValue(childObject);
                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    if (!AreEnumerablePropertiesEqual(parentPropertyValue, childPropertyValue))
                    {
                        propertyChanges.Add(new EntityPropertyChange { OldValue = parentPropertyValue, NewValue = childPropertyValue, PropertyName = property.Name });
                    }
                }
                else if (!parentPropertyValue.Equals(childPropertyValue))
                {
                    propertyChanges.Add(new EntityPropertyChange { OldValue = parentPropertyValue, NewValue = childPropertyValue, PropertyName = property.Name });
                }
            }
            return propertyChanges;
        }

        public T GetEntityObjectFromStream<T>(Stream stream)
        {
            T result;
            using (var streamReader = new StreamReader(stream, BufferWorker.Encoding))
            {
                var parentContent = streamReader.ReadToEnd();
                result = _fileService.ParseString<T>(parentContent);
            }
            return result;
        }
    }
}
