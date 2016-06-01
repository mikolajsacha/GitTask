using System.Collections.Generic;
using System.Windows;
using GitTask.Domain.Model.Project;

namespace GitTask.UI.MVVM.View.Elements
{
    public partial class AlsoKnownAsPopup
    {
        private bool _isLoading;
        private IEnumerable<ProjectMember> _projectMembers;

        public IEnumerable<ProjectMember> ProjectMembers
        {
            get { return _projectMembers; }
            set
            {
                _projectMembers = value;
                ProjectMembersItemsControl.ItemsSource = value;
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                LoaderGrid.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public AlsoKnownAsPopup()
        {
            InitializeComponent();
        }
    }
}