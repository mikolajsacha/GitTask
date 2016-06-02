using System;
using System.Collections.Generic;
using GitTask.Domain.Model.Project;

namespace GitTask.Domain.Model.Repository
{
    public class EntityHistory
    {
        public DateTime CreationDate { get; set; }
        public ProjectMember Author { get; set; }
        public List<EntityCommitChange> Changes { get; set; } 
    }
}
