using System.Collections.Generic;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Task;

namespace GitTask.Domain.Model.Repository.Merging
{
    public class MergingConflicts
    {
        public List<EntityConflict<Task.Task>> TaskConflicts;
        public List<EntityConflict<TaskState>> TaskStatesConflicts;
        public EntityConflict<List<ProjectMember>> ProjectMembersConfict;

        public MergingConflicts()
        {
            TaskConflicts = new List<EntityConflict<Task.Task>>();
        }
    }
}
