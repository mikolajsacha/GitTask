using System.Linq;
using GitTask.Domain.Model.Project;
using GitTask.UI.MVVM.ViewModel.TaskHistory.ChangesPartials;

namespace GitTask.UI.MVVM.Design.TaskHistory
{
    public class DesignAssignedMembersChangeViewModel : AssignedMembersChangeViewModel
    {
        public DesignAssignedMembersChangeViewModel() : base(new DesignProjectMembersViewModel().ProjectMembers,
                                                             new DesignProjectMembersViewModel().ProjectMembers.Concat(
                                                                 new [] {new ProjectMember("Janusz", "Januszewski"), }))
        {
        }
    }
}