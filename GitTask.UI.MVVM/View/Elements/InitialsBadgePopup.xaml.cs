using System.Collections.Generic;
using GitTask.Domain.Model.Project;

namespace GitTask.UI.MVVM.View.Elements
{
    public partial class InitialsBadgePopup
    {
        public IEnumerable<ProjectMember> ProjectMembers { get; }

        public InitialsBadgePopup(IEnumerable<ProjectMember> projectMembers)
        {
            ProjectMembers = projectMembers;
            InitializeComponent();
        }
    }
}