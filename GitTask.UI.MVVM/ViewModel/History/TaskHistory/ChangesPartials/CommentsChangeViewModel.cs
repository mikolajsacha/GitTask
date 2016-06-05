using System.Collections.Generic;

namespace GitTask.UI.MVVM.ViewModel.History.TaskHistory.ChangesPartials
{
    public class CommentsChangeViewModel : BaseCollectionChangeViewModel<string>
    {
        public CommentsChangeViewModel(IEnumerable<string> oldCommentsCollection,
            IEnumerable<string> newCommentsCollection)
            : base(oldCommentsCollection, newCommentsCollection)
        {
        }
    }
}