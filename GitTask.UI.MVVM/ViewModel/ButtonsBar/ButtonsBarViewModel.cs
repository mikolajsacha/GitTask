using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.View.ProjectSettings;
using GitTask.UI.MVVM.View.TaskDetails;

namespace GitTask.UI.MVVM.ViewModel.ButtonsBar
{
    public class ButtonsBarViewModel : ViewModelBase
    {
        private readonly RelayCommand _addTaskCommand;
        public ICommand AddTaskCommand => _addTaskCommand;

        private readonly RelayCommand _addTaskStateCommand;
        public ICommand AddTaskStateCommand => _addTaskStateCommand;

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

        public ButtonsBarViewModel(IProjectPathsReadonlyService projectPathsService)
        {
            _areButtonsEnabled = false;

            _addTaskCommand = new RelayCommand(OnAddTaskCommand);
            _addTaskStateCommand = new RelayCommand(onAddTaskStateCommand);

            projectPathsService.ProjectPathChanged += OnProjectPathChanged;
        }

        private void OnProjectPathChanged()
        {
            AreButtonsEnabled = true;
        }

        private void OnAddTaskCommand()
        {
            var addTaskWindow = new AddTaskWindow();
            addTaskWindow.Show();
        }

        private void onAddTaskStateCommand()
        {
            var addTaskStateWindow = new AddTaskStateWindow();
            addTaskStateWindow.Show();
        }
    }
}