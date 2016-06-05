using System.Linq;
using GitTask.Domain.Model.Project;
using GitTask.UI.MVVM.ViewModel.History;

namespace GitTask.UI.MVVM.Design.TaskHistory
{
    public class DesignProjectMembersChangeViewModel : ProjectMembersChangeViewModel
    {
        public DesignProjectMembersChangeViewModel() : base(new DesignProjectMembersViewModel().ProjectMembers,
                                                             new DesignProjectMembersViewModel().ProjectMembers.Concat(
                                                                 new [] {new ProjectMember("Janusz", "Januszewski"), }))
        {
        }
    }
}