using System.Collections.Generic;
using GitTask.Domain.Enum;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Repository.EntityHistory;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.ViewModel.History.TaskHistory.ChangesPartials;

namespace GitTask.UI.MVVM.ViewModel.History.TaskHistory
{
    public class CommitChangesViewModel
    {
        private readonly IQueryService<TaskState> _taskStateQueryService;
        public ProjectMember Author { get; private set; }
        public string CreationDate { get; private set; }

        public ProjectMembersChangeViewModel AssignedMembersChangeViewModel { get; private set; }
        public CommentsChangeViewModel CommentsChangeViewModel { get; private set; }
        public ContentChangeViewModel ContentChangeViewModel { get; private set; }
        public TaskPriorityChangeViewModel TaskPriorityChangeViewModel { get; private set; }
        public TaskStateChangeViewModel TaskStateChangeViewModel { get; private set; }

        public CommitChangesViewModel(EntityCommitChange commitChanges, IQueryService<TaskState> taskStateQueryService)
        {
            _taskStateQueryService = taskStateQueryService;
            Author = commitChanges.Author;
            CreationDate = commitChanges.Date.ToString("g");
            ResolvePropertyChanges(commitChanges);
        }

        private void ResolvePropertyChanges(EntityCommitChange commitChanges)
        {
            AssignedMembersChangeViewModel = null;
            CommentsChangeViewModel = null;
            ContentChangeViewModel = null;
            TaskPriorityChangeViewModel = null;
            TaskStateChangeViewModel = null;

            foreach (var propertyChange in commitChanges.PropertyChanges)
            {
                switch (propertyChange.PropertyName)
                {
                    case "Content":
                        ContentChangeViewModel = 
                            new ContentChangeViewModel((string)propertyChange.OldValue,
                                                       (string)propertyChange.NewValue);
                        break;
                    case "AssignedMembers":
                        AssignedMembersChangeViewModel =
                            new ProjectMembersChangeViewModel((IEnumerable<ProjectMember>)propertyChange.OldValue,
                                                               (IEnumerable<ProjectMember>)propertyChange.NewValue);
                        break;
                    case "Priority":
                        TaskPriorityChangeViewModel =
                            new TaskPriorityChangeViewModel((TaskPriority)propertyChange.OldValue,
                                                            (TaskPriority)propertyChange.NewValue);
                        break;
                    case "State":
                        TaskStateChangeViewModel =
                            new TaskStateChangeViewModel((string)propertyChange.OldValue,
                                                         (string)propertyChange.NewValue,
                                                         _taskStateQueryService);
                        break;
                    case "Comments":
                        CommentsChangeViewModel =
                            new CommentsChangeViewModel((IEnumerable<string>)propertyChange.OldValue,
                                                        (IEnumerable<string>)propertyChange.NewValue);
                        break;
                    default:
                        continue;
                }
            }
        }
    }
}