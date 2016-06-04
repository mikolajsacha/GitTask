using System.Collections.Generic;

namespace GitTask.UI.MVVM.ViewModel.TaskHistory
{
    public class CommentsChangeViewModel : BaseChangeViewModel<IEnumerable<string>>
    {
        public CommentsChangeViewModel(IEnumerable<string> oldValue, IEnumerable<string> newValue)
                                     : base(oldValue, newValue)
        { }
    }
}