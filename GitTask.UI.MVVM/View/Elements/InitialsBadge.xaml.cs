﻿using System.Windows.Input;
using GitTask.Domain.Model.Project;
using GitTask.UI.MVVM.Locator;

namespace GitTask.UI.MVVM.View.Elements
{
    public partial class InitialsBadge
    {
        private AlsoKnownAsPopup _popup;
        public InitialsBadge()
        {
            InitializeComponent();
        }

        private async void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_popup != null) return;
            var dataContext = DataContext as ProjectMember;
            if (dataContext == null) return;
            _popup = new AlsoKnownAsPopup(true) { IsLoading = true, MainProjectMember = dataContext };
            _popup.MouseLeave += (o, args) => RemovePopup();
            MainGrid.Children.Add(_popup);
            _popup.IsOpen = true;
            var resolvedUsers = await IocLocator.ProjectMembersSetsViewModel.Resolve(dataContext);
            _popup.ProjectMembers = resolvedUsers;
            _popup.IsLoading = false;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (_popup != null && !_popup.IsMouseOver && !_popup.IsLoading) RemovePopup();
        }

        private void RemovePopup()
        {
            MainGrid.Children.Remove(_popup);
            _popup = null;
        }

    }
}