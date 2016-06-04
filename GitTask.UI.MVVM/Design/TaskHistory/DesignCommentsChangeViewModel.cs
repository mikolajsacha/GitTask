using GitTask.UI.MVVM.ViewModel.TaskHistory;

namespace GitTask.UI.MVVM.Design.TaskHistory
{
    public class DesignCommentsChangeViewModel : CommentsChangeViewModel
    {
        public DesignCommentsChangeViewModel() : base(new [] { "komentarz 1" }, new [] { "komentarz 1", "komentarz 2" })
        {
           
        }
    }
}