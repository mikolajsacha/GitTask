using System.Windows;
using System.Windows.Input;
using GitTask.UI.MVVM.ViewModel.TaskDetails;

namespace GitTask.UI.MVVM.View.TaskDetails
{
    public partial class EditTaskWindow
    {
        public EditTaskWindow(EditTaskViewModel editTaskViewModel)
        {
            DataContext = editTaskViewModel;
            InitializeComponent();
            OkButton.Click += OkButtonOnClick;

            SelectPriorityGrid.MouseEnter += delegate { SelectPriorityPopup.IsOpen = true; };
            SelectPriorityGrid.MouseLeave += SelectPriorityGrid_OnMouseLeave;
            SelectPriorityPopup.MouseLeave += delegate { SelectPriorityPopup.IsOpen = false; };

            AssignedMembersInitialsList.MouseEnter += delegate { SelectAssignedMembersPopup.IsOpen = true; };
            AssignedMembersInitialsList.MouseLeave += AssignedMembersInitialsList_OnMouseLeave;
            SelectAssignedMembersPopup.MouseLeave += delegate { SelectAssignedMembersPopup.IsOpen = false; };

            SelectStateGrid.MouseEnter += delegate { SelectStatePopup.IsOpen = true; };
            SelectStateGrid.MouseLeave += SelectStateGrid_OnMouseLeave;
            SelectStatePopup.MouseLeave += delegate { SelectStatePopup.IsOpen = false; };
        }

        private void OkButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
            var editTaskViewModel = DataContext as EditTaskViewModel;

            if (editTaskViewModel != null && editTaskViewModel.OkCommand.CanExecute(new object()))
            {
                editTaskViewModel.OkCommand.Execute(new object());
            }
        }

        private void AssignedMembersInitialsList_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!SelectAssignedMembersPopup.IsMouseOver)
            {
                SelectAssignedMembersPopup.IsOpen = false;
            }
        }

        private void SelectPriorityGrid_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!SelectPriorityPopup.IsMouseOver)
            {
                SelectPriorityPopup.IsOpen = false;
            }
        }

        private void SelectStateGrid_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!SelectStatePopup.IsMouseOver)
            {
                SelectStatePopup.IsOpen = false;
            }
        }

        private void RemoveButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
            var editTaskViewModel = DataContext as EditTaskViewModel;

            if (editTaskViewModel != null && editTaskViewModel.DeleteCommand.CanExecute(new object()))
            {
                editTaskViewModel.DeleteCommand.Execute(new object());
            }
        }
    }
}