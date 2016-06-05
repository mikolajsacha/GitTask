using System.Collections.Generic;
using System.Linq;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Repository.ProjectHistory;
using GitTask.Domain.Model.Task;
using GitTask.UI.MVVM.ViewModel.History.ProjectHistory.ChangesPartials;

namespace GitTask.UI.MVVM.ViewModel.History.ProjectHistory
{
    public class ProjectCommitChangesViewModel
    {
        public ProjectMember Author { get; private set; }
        public string CreationDate { get; private set; }

        public ProjectMembersChangeViewModel ProjectMembersChangeViewModel { get; private set; }
        public TaskChangesViewModel TaskChangesViewModel { get; private set; }
        public TaskStatesChangesViewModel TaskStatesChangesViewModel { get; private set; }

        public ProjectCommitChangesViewModel(ProjectCommitChange commitChanges)
        {
            Author = commitChanges.Author;
            CreationDate = commitChanges.Date.ToString("g");
            ResolvePropertyChanges(commitChanges);
        }

        private void ResolvePropertyChanges(ProjectCommitChange commitChanges)
        {
            if (commitChanges.ProjectMembersChange != null)
            {
                ProjectMembersChangeViewModel =
                    new ProjectMembersChangeViewModel((IEnumerable<ProjectMember>)commitChanges.ProjectMembersChange.OldValue,
                        (IEnumerable<ProjectMember>)commitChanges.ProjectMembersChange.NewValue);
            }
            if (commitChanges.RemovedTasks.Any() || commitChanges.AddedTasks.Any())
            {
                TaskChangesViewModel = new TaskChangesViewModel(commitChanges.AddedTasks, commitChanges.RemovedTasks);
            }
            if (commitChanges.TaskStatesChange != null)
            {
                var orderedOld =
                    ((IEnumerable<TaskState>) commitChanges.TaskStatesChange.OldValue).OrderBy(x => x.Position);
                var orderedNew =
                    ((IEnumerable<TaskState>) commitChanges.TaskStatesChange.NewValue).OrderBy(x => x.Position);
                TaskStatesChangesViewModel = new TaskStatesChangesViewModel(orderedOld, orderedNew);
            }
        }
    }
}