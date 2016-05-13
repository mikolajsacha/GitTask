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
        }

        private void OkButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
            var setCurrentUserViewModel = DataContext as AddTaskViewModel;

            if (setCurrentUserViewModel != null && setCurrentUserViewModel.OkCommand.CanExecute(new object()))
            {
                setCurrentUserViewModel.OkCommand.Execute(new object());
            }
        }
    }
}