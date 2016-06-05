using System;
using System.Linq;
using GitTask.Domain.Model.Repository.ProjectHistory;
using GitTask.UI.MVVM.ViewModel.History.ProjectHistory;

namespace GitTask.UI.MVVM.Design.ProjectHistory
{
    public class DesignProjectCommitChangesViewModel : ProjectCommitChangesViewModel
    {
           public DesignProjectCommitChangesViewModel() : base(new ProjectCommitChange
           {
               Author = new DesignProjectMember(),
               Date = DateTime.Now,
               AddedTasks = new DesignTaskChangesViewModel().AddedTasks.ToList(),
               RemovedTasks = new DesignTaskChangesViewModel().RemovedTasks.ToList(),
           })
        {
          
        }
    }
}