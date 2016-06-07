using System.Collections.Generic;
using GitTask.Domain.Attributes;
using GitTask.Domain.Enum;
using GitTask.Domain.Model.Project;

namespace GitTask.Domain.Model.Task
{
    [Key("Title")]
    public class Task
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public TaskPriority Priority { get; set; }

        public string State { get; set; }
        public IEnumerable<ProjectMember> AssignedMembers { get; set; }
        public IList<string> Comments { get; set; }
    }
}
