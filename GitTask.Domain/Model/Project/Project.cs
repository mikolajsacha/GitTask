using System;
using GitTask.Domain.Attributes;
using GitTask.Domain.Model.Interface;

namespace GitTask.Domain.Model.Project
{
    [Key("Title")]
    public class Project : IWithCreationDate
    {
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
