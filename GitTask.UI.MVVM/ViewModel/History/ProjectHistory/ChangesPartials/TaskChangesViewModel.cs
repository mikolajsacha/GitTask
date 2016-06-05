using System.Collections.Generic;
using System.Linq;

namespace GitTask.UI.MVVM.ViewModel.History.ProjectHistory.ChangesPartials
{
    public class TaskChangesViewModel
    {
        public IEnumerable<string> AddedTasks { get; }
        public IEnumerable<string> RemovedTasks { get; }

        public bool AnyAddedTasks => AddedTasks.Any();
        public bool AnyRemovedTasks => RemovedTasks.Any();

        public TaskChangesViewModel(IEnumerable<string> addedTasks, IEnumerable<string> removedTasks)
        {
            AddedTasks = addedTasks;
            RemovedTasks = removedTasks;
        }
    }
}