using GitTask.UI.MVVM.ViewModel.History.ProjectHistory.ChangesPartials;

namespace GitTask.UI.MVVM.Design.ProjectHistory
{
    public class DesignTaskChangesViewModel : TaskChangesViewModel
    {
        public DesignTaskChangesViewModel() : base(new[] { "dodany task" }, new[] { "usuniety task" }) { }
    }
}