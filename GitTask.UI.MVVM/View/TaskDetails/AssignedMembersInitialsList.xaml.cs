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

        private void ScrollViewer_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollviewer = sender as ScrollViewer;
            if (scrollviewer == null) return;
            if (e.Delta > 0)
                scrollviewer.LineLeft();
            else
                scrollviewer.LineRight();
            e.Handled = true;
        }
    }
}