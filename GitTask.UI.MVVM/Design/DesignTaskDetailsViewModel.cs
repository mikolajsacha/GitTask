using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Task;

namespace GitTask.UI.MVVM.Design
{
    public class DesignTaskDetailsViewModel : ViewModelBase // based on GitTask.UI.MVVM.ViewModel.TaskDetails.TaskDetailsViewModel
    {
        public Task Task { get; }

        public DesignTaskDetailsViewModel(Task task)
        {
            Task = task;
        }

        public DesignTaskDetailsViewModel()
        {
            //TODO
        }
    }
}