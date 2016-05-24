using GitTask.Domain.Model.Project;

namespace GitTask.UI.MVVM.Model
{
    public class RegistryProjectInformation
    {
        public string ProjectPath { get; set; }
        public ProjectMember CurrentUser { get; set; }
    }
}