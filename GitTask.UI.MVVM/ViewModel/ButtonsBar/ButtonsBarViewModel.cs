using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.ButtonsBar
{
    public class ButtonsBarViewModel : ViewModelBase
    {
        private readonly RelayCommand _openProjectCommand;
        public ICommand OpenProjectCommand => _openProjectCommand;

        private readonly RelayCommand _addTaskCommand;
        public ICommand AddTaskCommand => _addTaskCommand;

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

        public ButtonsBarViewModel(IQueryService<Project> projectQueryService)
        {
            _areButtonsEnabled = false;
            projectQueryService.ElementAdded += ProjectQueryServiceOnElementAddedOrUpdated;
            projectQueryService.ElementUpdated += ProjectQueryServiceOnElementAddedOrUpdated;

            _openProjectCommand = new RelayCommand(OnOpenProjectCommand);
            _addTaskCommand = new RelayCommand(OnAddTaskCommand);
        }

        private void ProjectQueryServiceOnElementAddedOrUpdated(Project project)
        {
            AreButtonsEnabled = project.IsInitialized;
        }

        private void OnOpenProjectCommand()
        {
            throw new NotImplementedException();
        }

        private void OnAddTaskCommand()
        {
            throw new NotImplementedException();
        }
    }
}