using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.View.ProjectSettings;
using GitTask.UI.MVVM.View.TaskDetails;

namespace GitTask.UI.MVVM.ViewModel.ActionBar
{
    public class ButtonsViewModel : ViewModelBase
    {
        private readonly RelayCommand _addTaskCommand;
        public ICommand AddTaskCommand => _addTaskCommand;

        private readonly RelayCommand _addTaskStateCommand;
        public ICommand AddTaskStateCommand => _addTaskStateCommand;

        private readonly RelayCommand _setCurrentUserCommand;
        public ICommand SetCurrentUserCommand => _setCurrentUserCommand;

        private bool _areButtonsEnabled;
        public bool AreButtonsEnabled
        {
            get { return _areButtonsEnabled; }
            private set
            {
                _areButtonsEnabled = value;
                RaisePropertyChanged();
            }
        }

        public ButtonsViewModel(IProjectPathsReadonlyService projectPathsService)
        {
            _addTaskCommand = new RelayCommand(OnAddTaskCommand);
            _addTaskStateCommand = new RelayCommand(OnAddTaskStateCommand);
            _setCurrentUserCommand = new RelayCommand(OnSetCurrentUserCommand);

            projectPathsService.ProjectPathChanged += OnProjectPathChanged;
            _areButtonsEnabled = projectPathsService.IsProjectPathChosen;
        }

        private void OnProjectPathChanged()
        {
            AreButtonsEnabled = true;
        }

        private void OnAddTaskCommand()
        {
            var addTaskWindow = new AddTaskWindow {Owner = Application.Current.MainWindow};
            addTaskWindow.Show();
        }

        private void OnSetCurrentUserCommand()
        {
            var setCurrentUserWindow = new SetCurrentUserWindow {Owner = Application.Current.MainWindow};
            setCurrentUserWindow.Show();
        }

        private void OnAddTaskStateCommand()
        {
            var addTaskStateWindow = new AddTaskStateWindow {Owner = Application.Current.MainWindow};
            addTaskStateWindow.Show();
        }
    }
}