using System.Windows.Controls;
using GitTask.Repository.Model;
using GitTask.UI.MVVM.ViewModel.Elements;

namespace GitTask.UI.MVVM.View.Elements
{
    public partial class SelectUsersPartial
    {
        public SelectUsersPartial()
        {
            InitializeComponent();
            UsersList.SelectionChanged += UsersListOnSelectionChanged;
        }

        private void UsersListOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            foreach (var addedItem in selectionChangedEventArgs.AddedItems)
            {
                ((SelectUsersViewModel)DataContext).SelectedUsers.Add((ProjectMember)addedItem);
            }
            foreach (var removedItem in selectionChangedEventArgs.RemovedItems)
            {
                ((SelectUsersViewModel)DataContext).SelectedUsers.Remove((ProjectMember)removedItem);
            }
        }
    }
}