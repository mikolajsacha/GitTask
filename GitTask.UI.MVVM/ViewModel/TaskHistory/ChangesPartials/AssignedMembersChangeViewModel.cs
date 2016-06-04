using System.Collections.Generic;
using GitTask.Domain.Model.Project;

namespace GitTask.UI.MVVM.ViewModel.TaskHistory.ChangesPartials
{
    public class AssignedMembersChangeViewModel : BaseCollectionChangeViewModel<ProjectMember>
    {
        public AssignedMembersChangeViewModel(IEnumerable<ProjectMember> oldAssignedMembers,
            IEnumerable<ProjectMember> newAssignedMembers)
            : base(oldAssignedMembers, newAssignedMembers)
        {
        }
    }
}