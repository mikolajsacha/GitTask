using System;
using System.Collections.Generic;
using GitTask.UI.MVVM.ViewModel.History.ProjectHistory;
using Ph = GitTask.Domain.Model.Repository.ProjectHistory;

namespace GitTask.UI.MVVM.Design.ProjectHistory
{
    public class DesignProjectHistoryViewModel : ProjectHistoryViewModel
    {
        public DesignProjectHistoryViewModel() : base(new Ph.ProjectHistory() {
           CreationDate = DateTime.Now, Changes = new List<Ph.ProjectCommitChange>()
        }) 
        {

        }
    }
}