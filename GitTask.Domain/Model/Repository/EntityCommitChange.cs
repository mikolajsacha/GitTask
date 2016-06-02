using System;
using System.Collections.Generic;
using GitTask.Domain.Model.Project;

namespace GitTask.Domain.Model.Repository
{
    public class EntityCommitChange
    {
        public DateTime Date { get; set; }
        public ProjectMember Author { get; set; }
        public List<EntityPropertyChange> PropertyChanges { get; set; }
    }
}
