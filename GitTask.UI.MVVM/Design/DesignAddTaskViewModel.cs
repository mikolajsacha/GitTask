using GitTask.Domain.Enum;
using GitTask.UI.MVVM.ViewModel.TaskDetails;

namespace GitTask.UI.MVVM.Design
{
    public class DesignAddTaskViewModel : AddTaskViewModel
    {
        public DesignAddTaskViewModel() : base(null)
        {
            Title = "Jakis Tytul";
            Content = "Zawartosc";
            SelectUsersViewModel.SelectedUsers.Add(new DesignProjectMember());
            SelectTaskPriorityViewModel.SelectedTaskPriority = TaskPriority.Major;
        }
    }
}