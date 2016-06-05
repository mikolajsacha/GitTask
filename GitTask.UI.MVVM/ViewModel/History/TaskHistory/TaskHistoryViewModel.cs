using System.Collections.ObjectModel;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Repository.EntityHistory;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.History.TaskHistory
{
    public class TaskHistoryViewModel
    {
        private readonly IQueryService<TaskState> _taskStateQueryService;
        public ProjectMember Author { get; private set; }
        public string CreationDate { get; private set; }
        public ObservableCollection<CommitChangesViewModel> CommitChanges { get; } 

        public TaskHistoryViewModel(EntityHistory taskHistory, IQueryService<TaskState> taskStateQueryService)
        {
            _taskStateQueryService = taskStateQueryService;
            CommitChanges = new ObservableCollection<CommitChangesViewModel>();
            ResolveTaskHistory(taskHistory);
        }

        private void ResolveTaskHistory(EntityHistory taskHistory)
        {
            Author = taskHistory.Author;
            CreationDate = taskHistory.CreationDate.ToString("g");

            CommitChanges.Clear();

            foreach (var commitChange in taskHistory.Changes)
            {
                CommitChanges.Add(new CommitChangesViewModel(commitChange, _taskStateQueryService));
            }
        }
    }
}