using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GitTask.Domain.Model.Project;
using GitTask.UI.MVVM.ViewModel.Elements;

namespace GitTask.UI.MVVM.View.Elements
{
    public partial class SelectUsersPartial
    {
        public SelectUsersPartial()
        {
            InitializeComponent();
            UsersList.SelectionChanged += UsersListOnSelectionChanged;
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var dataContext = DataContext as SelectUsersViewModel;
            if (dataContext == null) return;

            UsersList.SelectionMode = (SelectionMode)Enum.Parse(typeof(SelectionMode), dataContext.SelectionMode);

            if (UsersList.SelectionMode == SelectionMode.Multiple)
            {
                UsersList.SelectionChanged -= UsersListOnSelectionChanged;
                UsersList.SelectedItems.Clear();
                foreach (var user in dataContext.SelectedUsers)
                {
                    UsersList.SelectedItems.Add(user);
                }
                UsersList.SelectionChanged += UsersListOnSelectionChanged;
                dataContext.SelectedUsers.CollectionChanged += SelectedUsersOnCollectionChanged;
            }
            else
            {
                UsersList.SelectionChanged -= UsersListOnSelectionChanged;
                UsersList.SelectedItem = dataContext.LastSelectedUser;
                UsersList.SelectionChanged += UsersListOnSelectionChanged;
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