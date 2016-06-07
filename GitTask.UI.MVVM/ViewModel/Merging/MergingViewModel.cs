using System.Linq;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Repository.Merging;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.Merging
{
    public class MergingViewModel : ViewModelBase
    {
        private readonly IMergingService _mergingService;
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

        public MergingViewModel(IMergingService mergingService,
                                IProjectPathsReadonlyService projectPathsService)
        {
            _mergingService = mergingService;
            _isLoading = mergingService.IsMergingCompleted;

            projectPathsService.ProjectPathChanged += ProjectPathsServiceOnProjectPathChanged;
            mergingService.MergingConflictsAquired += MergingServiceOnMergingConflictsAquired;
        }

        private void MergingServiceOnMergingConflictsAquired()
        {
            _mergingConflicts = _mergingService.MergingConflicts;
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
