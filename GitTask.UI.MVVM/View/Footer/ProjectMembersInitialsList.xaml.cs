using System.Windows.Controls;
using System.Windows.Input;

namespace GitTask.UI.MVVM.View.Footer
{
    public partial class ProjectMembersInitialsList
    {
        public ProjectMembersInitialsList()
        {
            InitializeComponent();
        }

        private void ScrollViewier_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
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