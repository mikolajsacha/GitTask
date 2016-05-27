using GalaSoft.MvvmLight;
using GitTask.Domain.Enum;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Task;

namespace GitTask.UI.MVVM.Design
{
    public class DesignTaskDetailsViewModel : ViewModelBase // based on GitTask.UI.MVVM.ViewModel.TaskDetails.TaskDetailsViewModel
    {
        public Task Task { get; }
        public bool IsVisible => true;

        public DesignTaskDetailsViewModel(Task task)
        {
            Task = task;
        }

        public DesignTaskDetailsViewModel()
        {
            Task = new Task
            {
                AssignedMembers = new[] { new ProjectMember("Jan Kowalski", "jankowalski@o2.pl"), new ProjectMember("Maciej Łoś", "losmaciej@hotmail.com"), },
                Title = "Zrobić to i owo.",
                Content = "Trzba koniecznie porobić to oraz owo!",
                Priority = TaskPriority.Blocker,
               State = "TO DO"
            };
        }
    }
}