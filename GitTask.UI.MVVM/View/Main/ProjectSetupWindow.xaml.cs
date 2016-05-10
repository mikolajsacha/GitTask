using System.Windows;
using GitTask.UI.MVVM.ViewModel.Main;

namespace GitTask.UI.MVVM.View.Main
{
    public partial class ProjectSetupWindow
    {
        public ProjectSetupWindow()
        {
            InitializeComponent();
            OkButton.Click += OkButtonOnClick;
        }

        private void OkButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
            var projectSetupViewModel = DataContext as ProjectSetupViewModel;
            if (projectSetupViewModel != null && projectSetupViewModel.OkCommand.CanExecute(new object()))
            {
                projectSetupViewModel.OkCommand.Execute(new object());
            }
        }
    }
}