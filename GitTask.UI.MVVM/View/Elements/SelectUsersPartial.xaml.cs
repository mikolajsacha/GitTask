using System.Collections.Specialized;
using System.Linq;
using System.Windows;
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
            OnDataContextChanged(null, new DependencyPropertyChangedEventArgs());
            UsersList.SelectionChanged += UsersListOnSelectionChanged;
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var dataContext = DataContext as SelectUsersViewModel;
            if (dataContext != null && dataContext.SelectionMode == "Multiple")
            {
                dataContext.SelectedUsers.CollectionChanged += SelectedUsersOnCollectionChanged;
            }
        }

        private void SelectedUsersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (notifyCollectionChangedEventArgs.NewItems != null)
            {
                foreach (var newItem in notifyCollectionChangedEventArgs.NewItems.Cast<object>().Where(newItem => !UsersList.SelectedItems.Contains(newItem)))
                {
                    UsersList.SelectedItems.Add(newItem);
                }
            }
            if (notifyCollectionChangedEventArgs.OldItems != null)
            {
                foreach (var oldItem in notifyCollectionChangedEventArgs.OldItems.Cast<object>().Where(oldItem => UsersList.SelectedItems.Contains(oldItem)))
                {
                    UsersList.SelectedItems.Remove(oldItem);
                }
            }
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