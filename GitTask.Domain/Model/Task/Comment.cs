using GitTask.Domain.Attributes;

namespace GitTask.Domain.Model.Task
{
    [Key("Id")]
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
