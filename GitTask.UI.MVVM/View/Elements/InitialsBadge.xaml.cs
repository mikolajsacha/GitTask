using System.Windows.Controls.Primitives;
using System.Windows.Input;
using GitTask.Domain.Model.Project;
using GitTask.UI.MVVM.Locator;

namespace GitTask.UI.MVVM.View.Elements
{
    public partial class InitialsBadge
    {
        private Popup _popup;

        public InitialsBadge()
        {
            InitializeComponent();
        }

        private async void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_popup != null) return;
            var dataContext = DataContext as ProjectMember;
            if (dataContext == null) return;

            var resolvedUsers = await IocLocator.ProjectMembersSetsViewModel.Resolve(dataContext);
            _popup = new InitialsBadgePopup(resolvedUsers);
            MainGrid.Children.Add(_popup);
            _popup.IsOpen = true;
            _popup.MouseLeave += (o, args) => RemovePopup();
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (_popup != null && !_popup.IsMouseOver) RemovePopup();
        }

        private void RemovePopup()
        {
            MainGrid.Children.Remove(_popup);
            _popup = null;
        }
    }
}