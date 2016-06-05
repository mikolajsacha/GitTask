using System.Linq;
using System.Windows.Media;
using GitTask.Domain.Model.Task;
using GitTask.UI.MVVM.ViewModel.History.ProjectHistory.ChangesPartials;

namespace GitTask.UI.MVVM.Design.ProjectHistory
{
    public class DesignTaskStatesChangesViewModel : TaskStatesChangesViewModel
    {
        public DesignTaskStatesChangesViewModel()
                                        : base(new DesignSelectTaskStateViewModel().AllTaskStates,
                                              new DesignSelectTaskStateViewModel().AllTaskStates.Concat(
                                                  new[] { new TaskState { Color = Brushes.Aquamarine, Name = "ADDED", Position = 10 } }))
        { }
    }
}