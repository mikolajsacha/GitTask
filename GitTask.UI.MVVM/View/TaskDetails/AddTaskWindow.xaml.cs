using System.Windows;
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

            SelectAssignedMembersButton.Click += delegate { SelectAssignedMembersPopup.IsOpen = !SelectAssignedMembersPopup.IsOpen; };
            SelectAssignedMembersPopup.MouseLeave += delegate { SelectAssignedMembersPopup.IsOpen = false; };
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
    }
}