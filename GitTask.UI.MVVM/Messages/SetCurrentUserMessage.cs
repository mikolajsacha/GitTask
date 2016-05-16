using GitTask.Domain.Model.Project;

namespace GitTask.UI.MVVM.Messages
{
    public class SetCurrentUserMessage
    {
        public ProjectMember CurrentUser { get; set; }
    }
}
