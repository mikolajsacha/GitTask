using System.Windows;
using GitTask.UI.MVVM.ViewModel.ProjectSettings;

namespace GitTask.UI.MVVM.View.ProjectSettings
{
    public partial class SetCurrentUserWindow
    {
        public SetCurrentUserWindow()
        {
            InitializeComponent();
            OkButton.Click += OkButtonOnClick;
        }

        private void OkButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
            var setCurrentUserViewModel = DataContext as SetCurrentUserViewModel;

            if (setCurrentUserViewModel != null && setCurrentUserViewModel.OkCommand.CanExecute(new object()))
            {
                setCurrentUserViewModel.OkCommand.Execute(new object());
            }
        }
    }
}