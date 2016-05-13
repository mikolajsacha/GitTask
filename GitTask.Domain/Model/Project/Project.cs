using System.Collections.Generic;
using GitTask.Domain.Attributes;

namespace GitTask.Domain.Model.Project
{
    [Key("Title")]
    public class Project
    {
        public string Title { get; set; }
        public List<string> ProjectMembersNotInRepository { get; set; }
    }
}
