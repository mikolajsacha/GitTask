using GitTask.Domain.Model.Project;

namespace GitTask.UI.MVVM.Messages
{
    public class AddUserMessage
    {
        public ProjectMember UserToBeAdded { get; set; }
    }
}
