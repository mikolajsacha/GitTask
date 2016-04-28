using GitTask.Domain.Attributes;

namespace GitTask.Domain.Model.Project
{
    [Key("Name")]
    public class ProjectMember
    {
        public string Name { get; set; }
    }
}
