using System;
using GitTask.Domain.Model.Project;
using GitTask.UI.MVVM.ViewModel.TaskHistory.ChangesPartials;

namespace GitTask.UI.MVVM.Design.TaskHistory
{
    public class DesignCommitChangesViewModel
    {
        public ProjectMember Author { get; private set; }
        public string CreationDate { get; private set; }

        public AssignedMembersChangeViewModel AssignedMembersChangeViewModel { get; set; }
        public CommentsChangeViewModel CommentsChangeViewModel { get; set; }
        public ContentChangeViewModel ContentChangeViewModel { get; set; }
        public TaskPriorityChangeViewModel TaskPriorityChangeViewModel { get; set; }
        public TaskStateChangeViewModel TaskStateChangeViewModel { get; set; }

        public DesignCommitChangesViewModel()
        {
            Author = new DesignProjectMember();
            CreationDate = DateTime.Now.ToString("g");
        }
    }
}