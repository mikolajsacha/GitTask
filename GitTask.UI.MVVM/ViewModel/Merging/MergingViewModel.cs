using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Repository.Merging;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.Merging
{
    public class MergingViewModel : ViewModelBase
    {
        private readonly IMergingService _mergingService;
        private readonly IRepositoryService _repositoryService;
        private readonly IStorageService<Project> _projectStorageService;
        private MergingConflicts _mergingConflicts;

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

        private Task _currentlyChosenTask;
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

        private int _currentTaskConflictIndex;
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

        private EntityConflict<TaskStatesCollectionViewModel> _taskStateConflict;
        public EntityConflict<TaskStatesCollectionViewModel> TaskStateConflict
        {
            get { return _taskStateConflict; }
            set
            {
                _taskStateConflict = value;
                RaisePropertyChanged();
            }
        }

        private IEnumerable<TaskState> _currentlyChosenTaskStates;
        [SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
        public IEnumerable<TaskState> CurrentlyChosenTaskStates
        {
            get { return _currentlyChosenTaskStates; }
            set
            {
                _currentlyChosenTaskStates = value;
                if (value != TaskStateConflict.AncestorValue.TaskStates)
                {
                    TaskStateConflict.AncestorValue.IsChosen = false;
                }
                if (value != TaskStateConflict.OurValue.TaskStates)
                {
                    TaskStateConflict.OurValue.IsChosen = false;
                }
                if (value != TaskStateConflict.TheirValue.TaskStates)
                {
                    TaskStateConflict.TheirValue.IsChosen = false;
                }

                IsOkButtonEnabled = _currentlyChosenTaskStates != null;
                RaisePropertyChanged();
            }
        }

        private readonly RelayCommand _okCommand;
        public ICommand OkCommand => _okCommand;

        private bool _isOkButtonEnabled;
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
                                IProjectPathsReadonlyService projectPathsService,
                                IRepositoryService repositoryService,
                                IStorageService<Project> projectStorageService)
        {
            _mergingService = mergingService;
            _repositoryService = repositoryService;
            _projectStorageService = projectStorageService;
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
                await _repositoryService.SaveInIndex(_currentlyChosenTask);
                OnTaskConflictResolved();
            }
            else if (TaskStateConflict != null && _currentlyChosenTaskStates != null)
            {
                foreach (var taskState in _currentlyChosenTaskStates)
                {
                    await _repositoryService.SaveInIndex(taskState);
                }

                OnTaskStatesConflictResolved();
            }
        }

        private async void MergingServiceOnMergingConflictsAquired()
        {
            _mergingConflicts = _mergingService.MergingConflicts;

            if (_mergingConflicts.ProjectMembersConfict != null)
            {
                await ResolveProjectMembersConflict();
            }
            if (_mergingConflicts.TaskStatesConflicts.Any())
            {
                TaskStateConflict = new EntityConflict<TaskStatesCollectionViewModel>
                {
                    OurValue = new TaskStatesCollectionViewModel(_mergingConflicts.TaskStatesConflicts.Select(x => x.OurValue).Where(x => x != null).OrderBy(x => x.Position)),
                    AncestorValue = new TaskStatesCollectionViewModel(_mergingConflicts.TaskStatesConflicts.Select(x => x.AncestorValue).Where(x => x != null).OrderBy(x => x.Position)),
                    TheirValue = new TaskStatesCollectionViewModel(_mergingConflicts.TaskStatesConflicts.Select(x => x.TheirValue).Where(x => x != null).OrderBy(x => x.Position))
                };
            }
            else
            {
                TaskStateConflict = null;
                OnTaskStatesConflictResolved();
            }
            IsLoading = false;
        }

        private void OnTaskStatesConflictResolved()
        {
            _mergingService.MarkMergedConflict();
            IsOkButtonEnabled = false;
            TaskStateConflict = null;
            if (_mergingConflicts.TaskConflicts.Any())
            {
                _currentTaskConflictIndex = 0;
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
        }

        private async System.Threading.Tasks.Task ResolveProjectMembersConflict()
        {
            // project members are resolved automatically by taking sum of project members sets from both versions
            var mergedProjectMembers =
                _mergingConflicts.ProjectMembersConfict.TheirValue.Concat(
                    _mergingConflicts.ProjectMembersConfict.OurValue).Distinct();
            var project = (await _projectStorageService.GetAll()).First();
            project.ProjectMembersNotInRepository = mergedProjectMembers.ToList();
            await _repositoryService.SaveInIndex(project);
            _mergingService.MarkMergedConflict();
        }

        private void OnTaskConflictResolved()
        {
            _mergingService.MarkMergedConflict();
            _currentTaskConflictIndex++;
            IsOkButtonEnabled = false;
            if (_mergingConflicts.TaskConflicts.Count <= _currentTaskConflictIndex)
            {
                CurrentTaskConflictViewModel = null;
            }
            else
            {
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
            _mergingConflicts = null;
            IsLoading = true;
        }
    }
}