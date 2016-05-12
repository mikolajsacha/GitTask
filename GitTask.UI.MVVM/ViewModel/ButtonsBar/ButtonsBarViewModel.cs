using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.Messages;

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

        public ButtonsBarViewModel()
        {
            _areButtonsEnabled = false;

            _openProjectCommand = new RelayCommand(OnOpenProjectCommand);
            _addTaskCommand = new RelayCommand(OnAddTaskCommand);

            Messenger.Default.Register<ProjectInitializedMessage>(this, OnProjectInitializedMessage);
        }

        private void OnProjectInitializedMessage(ProjectInitializedMessage projectInitializedMessage)
        {
            AreButtonsEnabled = true;
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