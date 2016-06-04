using System.Windows.Media;
using GitTask.Domain.Model.Task;
using GitTask.UI.MVVM.ViewModel.TaskHistory;

namespace GitTask.UI.MVVM.Design.TaskHistory
{
    public class DesignTaskStateChangeViewModel : TaskStateChangeViewModel
    {
        public DesignTaskStateChangeViewModel() : base(new TaskState { Name = "TO DO", Color = Brushes.Green, Position = 0 },
                                                       new TaskState { Name = "IN PROGRESS", Color = Brushes.Red, Position = 1 })
        {
           
        }
    }
}