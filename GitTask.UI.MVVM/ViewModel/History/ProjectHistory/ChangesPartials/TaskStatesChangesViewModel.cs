using System.Collections.Generic;
using System.Linq;
using GitTask.Domain.Model.Task;

namespace GitTask.UI.MVVM.ViewModel.History.ProjectHistory.ChangesPartials
{
    public class TaskStatesChangesViewModel : BaseCollectionChangeViewModel<TaskState>
    {
        public TaskStatesChangesViewModel(IEnumerable<TaskState> oldValue, IEnumerable<TaskState> newValue)
                                        : base(oldValue, newValue)
        { }
    }
}