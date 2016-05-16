using System.Windows.Controls;
using System.Windows.Input;
using GitTask.Domain.Model.Project;
using GitTask.UI.MVVM.ViewModel.Elements;

namespace GitTask.UI.MVVM.View.TaskDetails
{
    public partial class AssignedMembersInitialsList
    {
        public AssignedMembersInitialsList()
        {
            InitializeComponent();
        }

        private void InitialBadge_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var clickedProjectMember = (ProjectMember)((UserControl)sender).DataContext;
            ((SelectUsersViewModel)DataContext).SelectedUsers.Remove(clickedProjectMember);
        }
    }
}