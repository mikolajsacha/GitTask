using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Model.Repository.Merging;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.Merging
{
    public class MergingViewModel : ViewModelBase
    {
        private readonly IMergingService _mergingService;
        private readonly IRepositoryService _repositoryService;
        private MergingConflicts _mergingConflicts;
        private int _currentTaskConflictIndex;

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            private set
            {
                _isLoading = value;
                RaisePropertyChanged();
            }
        }

        private EntityConflict<TaskContentViewModel> _currentTaskConflictViewModel;
        public EntityConflict<TaskContentViewModel> CurrentTaskConflictViewModel
        {
            get { return _currentTaskConflictViewModel; }
            private set
            {
                _currentTaskConflictViewModel = value;
                RaisePropertyChanged();
            }
        }

        private Task _currentlyChosenTask;
        private bool _isOkButtonEnabled;

        public Task CurrentlyChosenTask
        {
            get { return _currentlyChosenTask; }
            set
            {
                _currentlyChosenTask = value;
                if (value != CurrentTaskConflictViewModel.AncestorValue.Task)
                {
                    CurrentTaskConflictViewModel.AncestorValue.IsChosen = false;
                }
                if (value != CurrentTaskConflictViewModel.OurValue.Task)
                {
                    CurrentTaskConflictViewModel.OurValue.IsChosen = false;
                }
                if (value != CurrentTaskConflictViewModel.TheirValue.Task)
                {
                    CurrentTaskConflictViewModel.TheirValue.IsChosen = false;
                }

                IsOkButtonEnabled = _currentlyChosenTask != null;
                RaisePropertyChanged();
            }
        }

        private readonly RelayCommand _okCommand;
        public ICommand OkCommand => _okCommand;

        public bool IsOkButtonEnabled
        {
            get { return _isOkButtonEnabled; }
            private set
            {
                _isOkButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public MergingViewModel(IMergingService mergingService,
                                IProjectPathsReadonlyService projectPathsService, IRepositoryService repositoryService)
        {
            _mergingService = mergingService;
            _repositoryService = repositoryService;
            _isLoading = mergingService.IsMergingCompleted;
            _isOkButtonEnabled = false;
            _okCommand = new RelayCommand(OnOkCommand);

            projectPathsService.ProjectPathChanged += ProjectPathsServiceOnProjectPathChanged;
            mergingService.MergingConflictsAquired += MergingServiceOnMergingConflictsAquired;
        }

        private async void OnOkCommand()
        {
            if (CurrentTaskConflictViewModel != null && _currentlyChosenTask != null)
            {
                OnTaskConflictResolved();
                await _repositoryService.SaveInIndex(_currentlyChosenTask);
            }
        }

        private void MergingServiceOnMergingConflictsAquired()
        {
            _mergingConflicts = _mergingService.MergingConflicts;
            if (_mergingConflicts.TaskConflicts.Any())
            {
                _currentTaskConflictIndex = 0;
                IsOkButtonEnabled = false;
                var currentTaskConflict = _mergingConflicts.TaskConflicts.First();
                CurrentTaskConflictViewModel = new EntityConflict<TaskContentViewModel>
                {
                    OurValue = new TaskContentViewModel(currentTaskConflict.OurValue),
                    TheirValue = new TaskContentViewModel(currentTaskConflict.TheirValue),
                    AncestorValue = new TaskContentViewModel(currentTaskConflict.AncestorValue)
                };
            }
            else
            {
                CurrentTaskConflictViewModel = null;
            }
            IsLoading = false;
        }

        public void OnTaskConflictResolved()
        {
            _mergingService.MarkMergedConflict();
            _currentTaskConflictIndex++;
            if (_mergingConflicts.TaskConflicts.Count <= _currentTaskConflictIndex)
            {
                CurrentTaskConflictViewModel = null;
                // TODO: przejdz do innych konfliktow
            }
            else
            {
                IsOkButtonEnabled = false;
                var currentTaskConflict = _mergingConflicts.TaskConflicts[_currentTaskConflictIndex];
                CurrentTaskConflictViewModel = new EntityConflict<TaskContentViewModel>
                {
                    OurValue = new TaskContentViewModel(currentTaskConflict.OurValue),
                    TheirValue = new TaskContentViewModel(currentTaskConflict.TheirValue),
                    AncestorValue = new TaskContentViewModel(currentTaskConflict.AncestorValue)
                };
            }
        }

        private void ProjectPathsServiceOnProjectPathChanged()
        {
            IsLoading = true;
        }
    }
}
