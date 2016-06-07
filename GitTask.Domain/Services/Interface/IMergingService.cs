using System;
using GitTask.Domain.Model.Repository.Merging;

namespace GitTask.Domain.Services.Interface
{
    public interface IMergingService
    {
        event Action MergingCompleted;
        event Action MergingConflictsAquired;
        bool IsMergingCompleted { get; }
        MergingConflicts MergingConflicts { get; }
        void MarkMergedConflict();
    }
}
