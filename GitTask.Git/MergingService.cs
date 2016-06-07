using System;
using GitTask.Domain.Model.Repository.Merging;
using GitTask.Domain.Services.Interface;

namespace GitTask.Git
{
    public class MergingService : IMergingService
    {
        private int _conflictToBeMerged;
        private readonly IRepositoryService _repositoryService;

        public event Action MergingCompleted;
        public event Action MergingConflictsAquired;
        public bool IsMergingCompleted { get; private set; }
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
                MergingCompleted?.Invoke();
            }
        }

        private async void RepositoryServiceOnRepositoryInitalized()
        {
            MergingConflicts = await _repositoryService.GetCurrentMergingConflicts();
            if (MergingConflicts == null)
            {
                IsMergingCompleted = true;
                MergingCompleted?.Invoke();
                return;
            }
            _conflictToBeMerged = MergingConflicts.TaskConflicts.Count + MergingConflicts.TaskStatesConflicts.Count;
            if (MergingConflicts.ProjectMembersConfict != null) _conflictToBeMerged++;

            MergingConflictsAquired?.Invoke();

            if (_conflictToBeMerged != 0) return;

            IsMergingCompleted = true;
            MergingCompleted?.Invoke();
        }
    }
}
