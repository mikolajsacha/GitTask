using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;
using Microsoft.Practices.ServiceLocation;

namespace GitTask.UI.MVVM.ViewModel.TaskDetails
{
    public class TaskDetailsViewModel : ViewModelBase
    {
        private IQueryService<Task> _taskQueryService;
        private IQueryService<ProjectMember> _projectMemberQueryService;

        public Task Task { get; }
        public ObservableCollection<ProjectMember> AssignedMembers { get; }

        public TaskDetailsViewModel(Task task)
        {
            Task = task;
            _taskQueryService = ServiceLocator.Current.GetInstance<IQueryService<Task>>();
            _projectMemberQueryService = ServiceLocator.Current.GetInstance<IQueryService<ProjectMember>>();
            AssignedMembers = new ObservableCollection<ProjectMember>(
                _projectMemberQueryService.GetAll().Where(x => task.AssignedMembers.Contains(x.Name)));
        }
    }
}