using System.Linq;
using GitTask.Domain.Model.Task;
using GitTask.UI.MVVM.ViewModel.History.ProjectHistory.ChangesPartials;

namespace GitTask.UI.MVVM.Design.ProjectHistory
{
    public class DesignTaskStatesChangesViewModel : TaskStatesChangesViewModel
    {
        public DesignTaskStatesChangesViewModel()
                                        : base(new DesignSelectTaskStateViewModel().AllTaskStates,
                                              new DesignSelectTaskStateViewModel().AllTaskStates.Concat(
                                                  new[] { new TaskState { Color = "#FF00FFFF", Name = "ADDED", Position = 10 } }))
        { }
    }
}