using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.Locator;
using GitTask.UI.MVVM.Properties;
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

        private bool _isHistoryBeingResolved;
        public bool IsHistoryBeingResolved
        {
            get { return _isHistoryBeingResolved; }
            private set
            {
                _isHistoryBeingResolved = value;
                RaisePropertyChanged();
            }
        }

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

        public ButtonsViewModel(IRepositoryService repositoryService, IMergingService mergingService)
        {
            _isHistoryBeingResolved = false;
            _repositoryService = repositoryService;

            _resolveHistoryCommand = new RelayCommand(OnResolveHistoryCommand);
            _addTaskStateCommand = new RelayCommand(OnAddTaskStateCommand);
            _setCurrentUserCommand = new RelayCommand(OnSetCurrentUserCommand);

            mergingService.MergingCompleted += OnProjectPathChanged;
            _areButtonsEnabled = mergingService.IsMergingCompleted;
        }

        private void OnProjectPathChanged()
        {
            AreButtonsEnabled = true;
        }

        private async void OnResolveHistoryCommand()
        {
            if (_isHistoryBeingResolved) return;
            IsHistoryBeingResolved = true;
            var projectHistory = await _repositoryService.GetProjectHistory();
            IsHistoryBeingResolved = false;
            if (projectHistory == null)
            {
                MessageBox.Show(IocLocator.ResourceManager.GetString("NoProjectHistoryInRepository"));
                return;
            }
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