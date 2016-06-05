using System.Collections.ObjectModel;
using Ph = GitTask.Domain.Model.Repository.ProjectHistory;

namespace GitTask.UI.MVVM.ViewModel.History.ProjectHistory
{
    public class ProjectHistoryViewModel
    {
        public string CreationDate { get; private set; }
        public ObservableCollection<ProjectCommitChangesViewModel> CommitChanges { get; } 

        public ProjectHistoryViewModel(Ph.ProjectHistory projectHistory)
        {
            CommitChanges = new ObservableCollection<ProjectCommitChangesViewModel>();
            ResolveTaskHistory(projectHistory);
        }

        private void ResolveTaskHistory(Ph.ProjectHistory projectHistory)
        {
            CreationDate = projectHistory.CreationDate.ToString("g");

            CommitChanges.Clear();

            foreach (var commitChange in projectHistory.Changes)
            {
                CommitChanges.Add(new ProjectCommitChangesViewModel(commitChange));
            }
        }
    }
}