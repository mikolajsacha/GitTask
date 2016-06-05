using System;
using System.Collections.Generic;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Repository.EntityHistory;

namespace GitTask.Domain.Model.Repository.ProjectHistory
{
    public class ProjectCommitChange
    {
        public DateTime Date { get; set; }
        public ProjectMember Author { get; set; }
        public List<string> AddedTasks { get; set; }
        public List<string> RemovedTasks { get; set; }
        public EntityPropertyChange ProjectMembersChange { get; set; }
        public EntityPropertyChange TaskStatesChange { get; set; }
    }
}
