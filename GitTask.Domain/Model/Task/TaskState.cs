using GitTask.Domain.Attributes;
using System.Windows.Media;

namespace GitTask.Domain.Model.Task
{
    [Key("Name")]
    public class TaskState
    {
        public string Name { get; set; }
        public int Position { get; set; }
        public Brush Color { get; set; }

        public override bool Equals(object obj)
        {
            var ts = obj as TaskState;
            if (ts == null)
            {
                return false;
            }

            return Name == ts.Name && Position == ts.Position;
        }

        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyMemberInGetHashCode
            return Name.GetHashCode() + Position.GetHashCode();
        }
    }
}
