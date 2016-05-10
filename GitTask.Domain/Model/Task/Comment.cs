using System;
using GitTask.Domain.Attributes;
using GitTask.Domain.Model.Interface;

namespace GitTask.Domain.Model.Task
{
    [Key("Id")]
    public class Comment : IWithAuthor
    {
        public int Id { get; set; }
        public string Content { get; set; }
        [ForeignKey]
        public string AuthorName { get; set; }
    }
}
