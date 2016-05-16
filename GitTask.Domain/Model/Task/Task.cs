using System.Collections.Generic;
using GitTask.Domain.Attributes;
using GitTask.Domain.Enum;
using GitTask.Domain.Model.Project;

namespace GitTask.Domain.Model.Task
{
    [Key("Id")]
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public IEnumerable<ProjectMember> AssignedMembers { get; set; }
        public TaskPriority Priority { get; set; }

        [ForeignKey]
        public string State { get; set; }

        [ForeignKey]
        public IEnumerable<int> CommentsIds { get; set; }
    }
}
