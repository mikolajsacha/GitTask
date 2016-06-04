namespace GitTask.UI.MVVM.ViewModel.TaskHistory
{
    public class ContentChangeViewModel : BaseChangeViewModel<string>
    {
        public ContentChangeViewModel(string oldValue, string newValue) : base(oldValue, newValue)
        { }
    }
}