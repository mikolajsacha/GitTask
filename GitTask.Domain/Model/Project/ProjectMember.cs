using System.Windows.Media;
using System.Windows.Media.Imaging;
using GitTask.Domain.Attributes;

namespace GitTask.Domain.Model.Project
{
    [Key("Name")]
    public class ProjectMember
    {
        public string Name { get; set; }
        public Brush Color { get; set; }
        public BitmapImage Avatar { get; set; }
    }
}
