using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.View.ProjectHistory;
using GitTask.UI.MVVM.View.ProjectSettings;
using GitTask.UI.MVVM.ViewModel.History.ProjectHistory;

namespace GitTask.UI.MVVM.ViewModel.ActionBar
{
    public class ButtonsViewModel : ViewModelBase
    {
        private readonly IRepositoryService _repositoryService;
        private readonly RelayCommand _addTaskStateCommand;
        public ICommand AddTaskStateCommand => _addTaskStateCommand;

        private readonly RelayCommand _setCurrentUserCommand;
        public ICommand SetCurrentUserCommand => _setCurrentUserCommand;

        private readonly RelayCommand _resolveHistoryCommand;
        public ICommand ResolveHistoryCommand => _resolveHistoryCommand;

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

        public ButtonsViewModel(IProjectPathsReadonlyService projectPathsService, IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;

            _resolveHistoryCommand = new RelayCommand(OnResolveHistoryCommand);
            _addTaskStateCommand = new RelayCommand(OnAddTaskStateCommand);
            _setCurrentUserCommand = new RelayCommand(OnSetCurrentUserCommand);

            projectPathsService.ProjectPathChanged += OnProjectPathChanged;
            _areButtonsEnabled = projectPathsService.IsProjectPathChosen;
        }

        private void OnProjectPathChanged()
        {
            AreButtonsEnabled = true;
        }

        private async void OnResolveHistoryCommand()
        {
            var projectHistory = await _repositoryService.GetProjectHistory();
            var projecHistoryViewModel = new ProjectHistoryViewModel(projectHistory);
            var projectHistoryWindow = new ProjectHistoryWindow(projecHistoryViewModel) { Owner = Application.Current.MainWindow };
            projectHistoryWindow.Show();
        }

        private void OnSetCurrentUserCommand()
        {
            var setCurrentUserWindow = new SetCurrentUserWindow { Owner = Application.Current.MainWindow };
            setCurrentUserWindow.Show();
        }

        private void OnAddTaskStateCommand()
        {
            var addTaskStateWindow = new AddTaskStateWindow { Owner = Application.Current.MainWindow };
            addTaskStateWindow.Show();
        }
    }
}