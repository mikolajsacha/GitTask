using GalaSoft.MvvmLight;
using GitTask.Domain.Enum;
using GitTask.Domain.Model.Task;
using GitTask.Repository.Model;

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
            Task = new Task
            {
                Id = 0,
                AssignedMembers = new[] { new ProjectMember("Jan Kowalski", "jankowalski@o2.pl"), new ProjectMember("Maciej Łoś", "losmaciej@hotmail.com"), },
                Title = "Zrobić to i owo.",
                Content = "Trzba koniecznie porobić to oraz owo!",
                Priority = TaskPriority.Blocker,
                CommentsIds = new[] { 0, 1 },
                State = "TO DO"
            };
        }
    }
}