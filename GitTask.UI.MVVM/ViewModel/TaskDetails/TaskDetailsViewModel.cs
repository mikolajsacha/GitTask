using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.TaskDetails
{
    public class TaskDetailsViewModel : ViewModelBase
    {
        private IQueryService<Task> _taskQueryService;
        private IQueryService<ProjectMember> _projectMemberQueryService;

        public Task Task { get; }
        public ProjectMember Author { get; }
        public ObservableCollection<ProjectMember> AssignedMembers { get; }

        public TaskDetailsViewModel(Task task, IQueryService<Task> taskQueryService, IQueryService<ProjectMember> projectMemberQueryService)
        {
            Task = task;
            _taskQueryService = taskQueryService;
            _projectMemberQueryService = projectMemberQueryService;
            AssignedMembers = new ObservableCollection<ProjectMember>();
            foreach (var assignedMemberName in task.AssignedMembers)
            {
                AssignedMembers.Add(_projectMemberQueryService.GetByKey(assignedMemberName));
            }
            Author = _projectMemberQueryService.GetByKey(task.AuthorName);
        }
    }
}