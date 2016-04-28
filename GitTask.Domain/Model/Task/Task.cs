using System;
using System.Collections.Generic;
using GitTask.Domain.Attributes;
using GitTask.Domain.Enum;
using GitTask.Domain.Model.Interface;

namespace GitTask.Domain.Model.Task
{
    [Key("Id")]
    public class Task : IWithCreationDate, IWithAuthor
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        [ForeignKey]
        public string AuthorName { get; set; }

        [ForeignKey]
        public IEnumerable<string> AssignedMembers { get; set; }

        public DateTime DateCreated { get; set; }
        public TaskPriority Priority { get; set; }

        [ForeignKey]
        public string State { get; set; }

        [ForeignKey]
        public IEnumerable<int> CommentsIds { get; set; }
    }
}
