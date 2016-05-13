using GitTask.Repository.Model;

namespace GitTask.UI.MVVM.Messages
{
    public class SetCurrentUserMessage
    {
        public ProjectMember CurrentUser { get; set; }
    }
}
