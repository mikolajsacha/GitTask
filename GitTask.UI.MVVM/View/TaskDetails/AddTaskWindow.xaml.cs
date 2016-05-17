using System;
using System.Windows;
using System.Windows.Input;
using GitTask.UI.MVVM.ViewModel.TaskDetails;

namespace GitTask.UI.MVVM.View.TaskDetails
{
    public partial class AddTaskWindow
    {
        public AddTaskWindow()
        {
            InitializeComponent();
            OkButton.Click += OkButtonOnClick;
            SelectPriorityGrid.MouseEnter += delegate { SelectPriorityPopup.IsOpen = true; };
            SelectPriorityPopup.MouseLeave += delegate { SelectPriorityPopup.IsOpen = false; };

            SelectStateGrid.MouseEnter += delegate { SelectStatePopup.IsOpen = true; };
            SelectStatePopup.MouseLeave += SelectStatePopupOnMouseLeave;  

            SelectAssignedMembersPopup.MouseLeave += delegate { SelectAssignedMembersPopup.IsOpen = false; };
        }

        private void SelectStatePopupOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!SelectStatePopup.IsMouseOver)
            {
                SelectStatePopup.IsOpen = false;
            }
        }

        private void OkButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
            var addTaskViewModel = DataContext as AddTaskViewModel;

            if (addTaskViewModel != null && addTaskViewModel.OkCommand.CanExecute(new object()))
            {
                addTaskViewModel.OkCommand.Execute(new object());
            }
        }

        private void AssignedMembersInitialsList_OnMouseEnter(object sender, MouseEventArgs e)
        {
            SelectAssignedMembersPopup.IsOpen = true;
        }

        private void AssignedMembersInitialsList_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!SelectAssignedMembersPopup.IsMouseOver)
            {
                SelectAssignedMembersPopup.IsOpen = false;
            }
        }
    }
}