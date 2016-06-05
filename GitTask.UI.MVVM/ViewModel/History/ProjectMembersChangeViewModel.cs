using System.Collections.Generic;
using GitTask.Domain.Model.Project;

namespace GitTask.UI.MVVM.ViewModel.History { 
    public class ProjectMembersChangeViewModel : BaseCollectionChangeViewModel<ProjectMember>
    {
        public ProjectMembersChangeViewModel(IEnumerable<ProjectMember> oldMembers,
            IEnumerable<ProjectMember> newMembers)
            : base(oldMembers, newMembers)
        {
        }
    }
}