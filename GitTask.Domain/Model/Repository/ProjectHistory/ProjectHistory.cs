using System;
using System.Collections.Generic;

namespace GitTask.Domain.Model.Repository.ProjectHistory
{
    public class ProjectHistory
    {
        public DateTime CreationDate { get; set; }
        public List<ProjectCommitChange> Changes { get; set; }
    }
}
