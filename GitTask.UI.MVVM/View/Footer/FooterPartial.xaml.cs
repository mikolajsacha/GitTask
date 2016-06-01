using System.Windows.Input;
using GitTask.UI.MVVM.Locator;
using GitTask.UI.MVVM.View.Elements;
using GitTask.UI.MVVM.ViewModel.Common;

namespace GitTask.UI.MVVM.View.Footer
{
    public partial class FooterPartial
    {
        private AlsoKnownAsPopup _popup;

        public FooterPartial()
        {
            InitializeComponent();
        }

        private async void CurrentUserNameOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_popup != null) return;
            var dataContext = CurrentUserTextBlock.DataContext as CurrentUserViewModel;
            if (dataContext?.CurrentUser == null) return;

            _popup = new AlsoKnownAsPopup {IsLoading = true};
            _popup.MouseLeave += (o, args) => RemovePopup();
            MainGrid.Children.Add(_popup);
            _popup.IsOpen = true;
            var resolvedUsers = await IocLocator.ProjectMembersSetsViewModel.Resolve(dataContext.CurrentUser);
            _popup.ProjectMembers = resolvedUsers;
            _popup.IsLoading = false;
        }

        private void CurrentUserNameOnMouseLeave(object sender, MouseEventArgs e)
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