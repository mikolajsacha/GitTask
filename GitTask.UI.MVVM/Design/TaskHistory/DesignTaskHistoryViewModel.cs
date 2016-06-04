using System;
using System.Collections.Generic;
using GitTask.Domain.Enum;
using GitTask.Domain.Model.Repository;
using GitTask.UI.MVVM.ViewModel.TaskHistory;

namespace GitTask.UI.MVVM.Design.TaskHistory
{
    public class DesignTaskHistoryViewModel : TaskHistoryViewModel
    {
        public DesignTaskHistoryViewModel() : base(new EntityHistory
        {
            Author = new DesignProjectMember(),
            CreationDate = DateTime.Now,
            Changes = new List<EntityCommitChange>
            {
                new EntityCommitChange
                {
                    Author = new DesignProjectMember(),
                    Date = DateTime.Today,
                    PropertyChanges = new List<EntityPropertyChange>
                    {
                       new EntityPropertyChange { OldValue = "stara", NewValue = "nowa", PropertyName = "Content"} ,
                       new EntityPropertyChange { OldValue = TaskPriority.Critical, NewValue = TaskPriority.Blocker, PropertyName = "Priority"} 
                    }
                }
            }
        }, null)
        {

        }

        private void ResolveTaskHistory(EntityHistory taskHistory)
        {
        }
    }
}