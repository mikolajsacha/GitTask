using System;
using System.Linq;
using GitTask.Domain.Model.Repository.Merging;
using GitTask.Domain.Services.Interface;

namespace GitTask.Git
{
    public class MergingService : IMergingService
    {
        private int _conflictToBeMerged;
        private readonly IRepositoryService _repositoryService;
        private bool _isMergingCompleted;

        public event Action MergingCompleted;
        public event Action MergingConflictsAquired;

        public bool IsMergingCompleted
        {
            get { return _isMergingCompleted; }
            private set
            {
                _isMergingCompleted = value;
                if (value)
                {
                    MergingCompleted?.Invoke();
                }
            }
        }

        public MergingConflicts MergingConflicts { get; private set; }

        public MergingService(IRepositoryService repositoryService, IProjectPathsReadonlyService projectPathsService)
        {
            IsMergingCompleted = false;
            _conflictToBeMerged = 0;
            _repositoryService = repositoryService;
            projectPathsService.ProjectPathChanged += ProjectPathsServiceOnProjectPathChanged;
            _repositoryService.RepositoryInitalized += RepositoryServiceOnRepositoryInitalized;
        }

        private void ProjectPathsServiceOnProjectPathChanged()
        {
            IsMergingCompleted = false;
        }

        public void MarkMergedConflict()
        {
            _conflictToBeMerged--;
            if (_conflictToBeMerged < 1)
            {
                IsMergingCompleted = true;
            }
        }

        private async void RepositoryServiceOnRepositoryInitalized()
        {
            MergingConflicts = await _repositoryService.GetCurrentMergingConflicts();
            if (MergingConflicts == null)
            {
                IsMergingCompleted = true;
                return;
            }
            _conflictToBeMerged = MergingConflicts.TaskConflicts.Count + (MergingConflicts.TaskStatesConflicts.Any() ? 1 : 0);
            if (MergingConflicts.ProjectConfict != null) _conflictToBeMerged++;

            MergingConflictsAquired?.Invoke();

            if (_conflictToBeMerged != 0) return;

            IsMergingCompleted = true;
        }
    }
}
