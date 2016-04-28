using GitTask.Domain.Attributes;
using System.Windows.Media;

namespace GitTask.Domain.Model.Task
{
    [Key("Name")]
    public class TaskState
    {
        public string Name { get; set; }
        public double Position { get; set; }

        public Brush Color { get; set; }
    }
}
